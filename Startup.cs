using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using WebAPIAuthors.Filters;
using WebAPIAuthors.Middlewares;
using WebAPIAuthors.Services;

namespace WebAPIAuthors
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers(options => options.Filters.Add(typeof(MyGlobalExceptionFilterAttribute)))
        .AddJsonOptions(options =>
        {
          options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        }).AddNewtonsoftJson();

      services.AddEndpointsApiExplorer();

      services.AddSwaggerGen(e =>
      {
        e.AddSecurityDefinition(name: "Bearer", new OpenApiSecurityScheme
        {
          Name = "Authorization",
          Type = SecuritySchemeType.ApiKey,
          Scheme = "Bearer",
          BearerFormat = "JWT",
          In = ParameterLocation.Header
        });

        e.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
          {
            new OpenApiSecurityScheme()
            {
              Reference = new OpenApiReference()
              {
                Type= ReferenceType.SecurityScheme,
                Id = "Bearer"
              }
            },

            new string[]{}
          }
        });

      });

      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(Configuration["ConnectionStrings:Production"]);
      });

      services.AddTransient<IService, ServiceA>();

      services.AddTransient<ServiceTransient>();
      services.AddScoped<ServiceScoped>();
      services.AddSingleton<ServiceSingleton>();

      services.AddResponseCaching();

      services.AddMemoryCache();

      services.AddTransient<MyActionFilterAttribute>();

      services.AddHostedService<WriteLogInFile>();

      services.AddAutoMapper(typeof(Startup));

      services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = false,
          ValidateAudience = false,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
          ClockSkew = TimeSpan.Zero
        });

      services.AddAuthorization(options =>
      {
        options.AddPolicy("isAdmin", policy =>
        {
          policy.RequireClaim("isAdmin", "1");
        });
      });

      services.AddCors(options =>
      {
        options.AddDefaultPolicy(policy =>
        {
          policy.WithOrigins(new string[] { "https://restninja.io" }).AllowAnyMethod().AllowAnyHeader();
        });
      });

      services.AddDataProtection();

      services.AddTransient<HashService>();

      services.AddTransient<LinksGeneratorService>();
      services.AddTransient<HATEOASAuthorFilterAttribute>();
      services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
      
      app.Map("/api/blocked", app =>
      {
        app.Run(async handler =>
        {
          await handler.Response.WriteAsync("I'm intercepting the pipe");
        });
      });

      /*
       //This is the middleware that log HTTP responses using app.Use
      //It's comment because was created in it's own class

      app.Use(async(context, next)=> {
        using (MemoryStream ms = new MemoryStream())
        {
          var originalBodyResponse = context.Response.Body;
          context.Response.Body = ms;
          await next.Invoke();

          ms.Seek(0, SeekOrigin.Begin);
          var response = new StreamReader(ms).ReadToEnd();
          ms.Seek(0, SeekOrigin.Begin);

          await ms.CopyToAsync(originalBodyResponse);
          context.Response.Body = originalBodyResponse;
          logger.LogInformation(response);
        }
      });
      */

      //This is one way that call the middleware that log the HTTP responses in output
      //app.UseMiddleware<LogResponsesHTTPMiddleware>();


      app.UseLogResponsesHttps();
      
      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors();

      app.UseResponseCaching();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

    }
  }
}
