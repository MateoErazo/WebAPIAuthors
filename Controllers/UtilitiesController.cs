using Microsoft.AspNetCore.Mvc;
using WebAPIAuthors.Services;

namespace WebAPIAuthors.Controllers
{

  [ApiController]
  [Route("api/utilities")]
  public class UtilitiesController:ControllerBase
  {
    private readonly IService service;
    private readonly ServiceTransient serviceTransient;
    private readonly ServiceScoped serviceScoped;
    private readonly ServiceSingleton serviceSingleton;

    public UtilitiesController(
      IService service,
      ServiceTransient serviceTransient,
      ServiceScoped serviceScoped,
      ServiceSingleton serviceSingleton) 
    {
      this.service = service;
      this.serviceTransient = serviceTransient;
      this.serviceScoped = serviceScoped;
      this.serviceSingleton = serviceSingleton;
    }


    /// <summary>
    /// It allow us can to see the diferent services types of .Net and his time life
    /// </summary>
    /// <returns>The Guids for every service type</returns>
    [HttpGet("services",Name ="getGUIDs")]
    public ActionResult GetGUIDs()
    {
      return Ok(new
      {
        UtiliesController_Transient = serviceTransient.Guid,
        ServiceA_Transient = service.GetGuidTransient(),
        UtilitiesController_Scoped = serviceScoped.Guid,
        ServiceA_Scoped = service.GetGuidScoped(),
        UtilitiesController_Singleton = serviceSingleton.Guid,
        ServiceA_Singleton = service.GetGuidSingleton()
      });
    }
  }
}
