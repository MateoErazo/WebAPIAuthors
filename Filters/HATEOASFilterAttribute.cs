using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAuthors.Filters
{
  public class HATEOASFilterAttribute:ResultFilterAttribute
  {

    protected bool MustIncludeHATEOAS(ResultExecutingContext context)
    {
      var response = context.Result as ObjectResult;

      if (!IsSuccessfulResponse(response))
      {
        return false;
      }

      var headers = context.HttpContext.Request.Headers["includeHATEOAS"];

      if (headers.Count == 0)
      {
        return false;
      }

      string valueHeader = headers[0];

      if (!valueHeader.Equals("Y",StringComparison.InvariantCultureIgnoreCase))
      {
        return false;
      }

      return true;
    }

    protected bool IsSuccessfulResponse(ObjectResult result)
    {
      if(result == null || result.Value is null)
      {
        return false;
      }

      if (result.StatusCode.HasValue && !result.StatusCode.ToString().StartsWith("2"))
      {
        return false;
      }

      return true;
    }
  }
}
