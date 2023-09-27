using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAuthors.Filters
{
  public class MyGlobalExceptionFilterAttribute:ExceptionFilterAttribute
  {
    private readonly string nameFile = "logError.txt";
    StreamWriter writer;
    Timer timer;

    private readonly ILogger<MyGlobalExceptionFilterAttribute> logger;
    private readonly IWebHostEnvironment env;

    public MyGlobalExceptionFilterAttribute(
      ILogger<MyGlobalExceptionFilterAttribute> logger,
      IWebHostEnvironment env) 
    {
      this.logger = logger;
      this.env = env;
    }

    public override void OnException(ExceptionContext context)
    {
      logger.LogError("Message from GlobalExceptionFilter...");
      logger.LogError(context.Exception, context.Exception.Message);
      Write($"-----ERROR FILTER EXCEPTION {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}-----");
      Write(context.Exception.ToString());
      Write(context.Exception.Message);
      base.OnException(context);
    }

    private void Write(string message)
    {
      string path = @$"{env.ContentRootPath}\wwwroot\{nameFile}";
      using (writer = new StreamWriter(path,true))
      {
        writer.WriteLine(message);
      }
    }

  }
}
