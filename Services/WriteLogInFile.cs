namespace WebAPIAuthors.Services
{
  public class WriteLogInFile : IHostedService
  {

    private readonly string nameFile = "logWebAPIAuthors.txt";
    private readonly IWebHostEnvironment env;
    Timer timer;
    StreamWriter writer;
    public WriteLogInFile(IWebHostEnvironment env) 
    {
      this.env = env;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Write("Starting process...");
      timer = new Timer(DoWork,null,TimeSpan.Zero,TimeSpan.FromSeconds(5));
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      timer.Dispose();
      Write("The process was stopped");
      return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
      Write($"Executing process {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}...");
    }

    private void Write(string message)
    {
      string path = $@"{env.ContentRootPath}\wwwroot\{nameFile}";

      using (writer = new StreamWriter(path:path, append:true))
      {
        writer.WriteLine(message);
      }
    }

    
  }
}
