namespace WebAPIAuthors.Services
{
  /// <summary>
  /// This interface was designed to can to see the services types on .Net
  /// and his time life
  /// </summary>
  public interface IService
  {
    public Guid GetGuidTransient();
    public Guid GetGuidScoped();
    public Guid GetGuidSingleton();

  }

  public class ServiceA : IService
  {
    private readonly ServiceTransient serviceTransient;
    private readonly ServiceScoped serviceScoped;
    private readonly ServiceSingleton serviceSingleton;

    public ServiceA(ServiceTransient serviceTransient,
      ServiceScoped serviceScoped,
      ServiceSingleton serviceSingleton)
    {
      this.serviceTransient = serviceTransient;
      this.serviceScoped = serviceScoped;
      this.serviceSingleton = serviceSingleton;
    }

    public Guid GetGuidScoped()
    {
      return serviceScoped.Guid;
    }

    public Guid GetGuidSingleton()
    {
      return serviceSingleton.Guid;
    }

    public Guid GetGuidTransient()
    {
      return serviceTransient.Guid;
    }
  }


  public class ServiceTransient
  {
    public Guid Guid = Guid.NewGuid();
  }

  public class ServiceScoped
  {
    public Guid Guid = Guid.NewGuid();
  }

  public class ServiceSingleton
  {
    public Guid Guid = Guid.NewGuid();
  }
}
