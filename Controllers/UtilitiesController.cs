using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebAPIAuthors.Entities;
using WebAPIAuthors.Filters;
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
    private readonly IMemoryCache cache;
    private readonly ApplicationDbContext context;
    private readonly ILogger<UtilitiesController> logger;

    public UtilitiesController(
      IService service,
      ServiceTransient serviceTransient,
      ServiceScoped serviceScoped,
      ServiceSingleton serviceSingleton,
      IMemoryCache cache,
      ApplicationDbContext context,
      ILogger<UtilitiesController> logger) 
    {
      this.service = service;
      this.serviceTransient = serviceTransient;
      this.serviceScoped = serviceScoped;
      this.serviceSingleton = serviceSingleton;
      this.cache = cache;
      this.context = context;
      this.logger = logger;
    }


    /// <summary>
    /// It allow us can to see the diferent services types of .Net and his time life
    /// </summary>
    /// <returns>The Guids for every service type</returns>
    [ResponseCache(Duration = 10)]
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

    [HttpGet("cache/{authorId:int}",Name="testCache")]
    public async Task<ActionResult<Author>> TestCache(int authorId)
    {

      if (!cache.TryGetValue(authorId,out Author author))
      {
        Author authorDb = await context.Authors.FirstOrDefaultAsync(x => x.Id == authorId);

        if (authorDb is null)
        {
          return NoContent();
        }

        cache.Set(authorId,authorDb);
        return authorDb;
      }

      return author;
    }

    [HttpGet("actionFilter",Name ="testActionFilter")]
    [ServiceFilter(typeof(MyActionFilterAttribute))]
    public ActionResult TestActionFilter()
    {
      logger.LogInformation("Doing things...");
      return NoContent();
    }

    [HttpGet("globalExceptionFilter",Name ="testGlobalExceptionFilter")]
    public ActionResult TestGlobalExceptionFilter()
    {
      throw new NotImplementedException();
    }
  }
}
