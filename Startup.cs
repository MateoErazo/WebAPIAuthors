using Microsoft.EntityFrameworkCore;
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
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      services.AddDbContext<ApplicationDbContext>(options =>
      {
        options.UseSqlServer(Configuration["ConnectionStrings:Production"]);
      });

      services.AddTransient<IService,ServiceA>();

      services.AddTransient<ServiceTransient>();
      services.AddScoped<ServiceScoped>();
      services.AddSingleton<ServiceSingleton>();
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

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

    }
  }
}
