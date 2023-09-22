using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPIAuthors.DTOs.HATEOAS;

namespace WebAPIAuthors.Controllers.V1
{

    [ApiController]
    [Route("api/v1")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }

        [AllowAnonymous]
        [HttpGet("roots", Name = "getRoots")]
        public async Task<ActionResult<IEnumerable<DataHATEOAS>>> GetRoots()
        {
            List<DataHATEOAS> resources = new List<DataHATEOAS>();
            resources.Add(new DataHATEOAS(link: Url.Link("getRoots", new { }), description: "self", method: "GET"));

            bool isAuthenticated = User.Identity.IsAuthenticated;
            var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

            if (isAuthenticated)
            {
                resources.Add(new DataHATEOAS(link: Url.Link("getAllAuthors", new { }), description: "list-of-authors", method: "GET"));
                resources.Add(new DataHATEOAS(link: Url.Link("getAllBooks", new { }), description: "list-of-books", method: "GET"));
            }

            if (isAdmin.Succeeded)
            {
                resources.Add(new DataHATEOAS(link: Url.Link("addNewAuthor", new { }), description: "add-new-author", method: "POST"));
                resources.Add(new DataHATEOAS(link: Url.Link("addNewBook", new { }), description: "add-new-book", method: "POST"));
            }

            return resources;
        }
    }
}
