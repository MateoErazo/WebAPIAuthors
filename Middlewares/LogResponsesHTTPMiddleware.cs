using Microsoft.Extensions.Logging;

namespace WebAPIAuthors.Middlewares
{
  public static class LogResponsesHTTPMiddlewareExtensions
  {
    public static IApplicationBuilder UseLogResponsesHttps(this IApplicationBuilder app)
    {
      return app.UseMiddleware<LogResponsesHTTPMiddleware>();
    }
  }

  /// <summary>
  /// This middleware help us to log all HTTP responses in Output
  /// </summary>
  public class LogResponsesHTTPMiddleware
  {
    private readonly RequestDelegate next;
    private readonly ILogger<LogResponsesHTTPMiddleware> logger;

    public LogResponsesHTTPMiddleware(
      RequestDelegate next,
      ILogger<LogResponsesHTTPMiddleware> logger)
    {
      this.next = next;
      this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) 
    {
      using (MemoryStream ms = new MemoryStream())
      {
        var originalBodyResponse = context.Response.Body;
        context.Response.Body = ms;
        await next(context);

        ms.Seek(0, SeekOrigin.Begin);
        var response = new StreamReader(ms).ReadToEnd();
        ms.Seek(0, SeekOrigin.Begin);

        await ms.CopyToAsync(originalBodyResponse);
        context.Response.Body = originalBodyResponse;
        logger.LogInformation(response);
      }
    }

  }
}
