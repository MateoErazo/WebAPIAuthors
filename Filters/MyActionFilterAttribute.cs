using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAuthors.Filters
{
  public class MyActionFilterAttribute : IActionFilter
  {
    private readonly ILogger<MyActionFilterAttribute> logger;

    public MyActionFilterAttribute(ILogger<MyActionFilterAttribute> logger) 
    {
      this.logger = logger;
    }
    public void OnActionExecuting(ActionExecutingContext context)
    {
      logger.LogInformation("Excuting filter before the Action...");
    }
    public void OnActionExecuted(ActionExecutedContext context)
    {
      logger.LogInformation("Excuting filter after the Action...");
    }

  }
}
