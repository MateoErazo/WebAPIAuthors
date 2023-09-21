using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Runtime.CompilerServices;
using WebAPIAuthors.DTOs.Author;
using WebAPIAuthors.DTOs.HATEOAS;

namespace WebAPIAuthors.Services
{
  public class LinksGeneratorService
  {
    private readonly IAuthorizationService authorizationService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IActionContextAccessor actionContextAccessor;

    public LinksGeneratorService(
      IAuthorizationService authorizationService,
      IHttpContextAccessor httpContextAccessor,
      IActionContextAccessor actionContextAccessor) {
      this.authorizationService = authorizationService;
      this.httpContextAccessor = httpContextAccessor;
      this.actionContextAccessor = actionContextAccessor;
    }

    private IUrlHelper BuildURLHelper()
    {
      var factory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<IUrlHelperFactory>();
      return factory.GetUrlHelper(actionContextAccessor.ActionContext);
    }

    private async Task<bool> IsAdmin()
    {
      var httpContext = httpContextAccessor.HttpContext;
      var isAdmin = await authorizationService.AuthorizeAsync(httpContext.User, "isAdmin");
      return isAdmin.Succeeded;
    }

    private bool IsAuthenticated()
    {
      var httpContext = httpContextAccessor.HttpContext;
      var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
      return isAuthenticated;
    }

    public async Task GenerateLinksAuthor(AuthorWithBooksDTO authorWithBooksDTO)
    {
      List<DataHATEOAS> resourcesAuthor = new List<DataHATEOAS>();

      var Url = BuildURLHelper();

      bool isAuthenticated = IsAuthenticated();
      var isAdmin = await IsAdmin();

      if (isAuthenticated)
      {
        resourcesAuthor.Add(new DataHATEOAS(link: Url.Link("getSingleAuthorById", new { id = authorWithBooksDTO.Id }),
        description: "self",
        method: "GET"));
      }

      if (isAdmin)
      {
        resourcesAuthor.Add(new DataHATEOAS(link: Url.Link("updateCompleteAuthor", new { id = authorWithBooksDTO.Id }),
        description: "update-complete-author",
        method: "PUT"));

        resourcesAuthor.Add(new DataHATEOAS(link: Url.Link("deleteAuthor", new { id = authorWithBooksDTO.Id }),
        description: "delete-author",
          method: "DELETE"));

        resourcesAuthor.Add(new DataHATEOAS(link: Url.Link("updatePartialAuthor", new { id = authorWithBooksDTO.Id }),
          description: "update-partial-author",
          method: "PATCH"));
      }

      authorWithBooksDTO.Resources = resourcesAuthor;

    }
  }
}
