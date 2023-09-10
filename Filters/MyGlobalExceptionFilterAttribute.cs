using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAuthors.Filters
{
  public class MyGlobalExceptionFilterAttribute:ExceptionFilterAttribute
  {
    private readonly ILogger<MyGlobalExceptionFilterAttribute> logger;

    public MyGlobalExceptionFilterAttribute(
      ILogger<MyGlobalExceptionFilterAttribute> logger) 
    {
      this.logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
      logger.LogError("Message from GlobalExceptionFilter...");
      logger.LogError(context.Exception, context.Exception.Message);
      base.OnException(context);
    }

  }
}
