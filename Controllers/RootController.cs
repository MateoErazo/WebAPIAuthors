using Microsoft.AspNetCore.Mvc;
using WebAPIAuthors.DTOs.HATEOAS;

namespace WebAPIAuthors.Controllers
{

  [ApiController]
  [Route("api")]
  public class RootController:ControllerBase
  {
    [HttpGet("roots",Name ="getRoots")]
    public ActionResult<IEnumerable<DataHATEOAS>> GetRoots()
    {
      List<DataHATEOAS> resources = new List<DataHATEOAS>();
      resources.Add(new DataHATEOAS(link: Url.Link("getRoots", new {}),description:"self",method:"GET"));
      return resources;
    }
  }
}
