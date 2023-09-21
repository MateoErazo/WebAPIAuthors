using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebAPIAuthors.DTOs.Author;
using WebAPIAuthors.Services;

namespace WebAPIAuthors.Filters
{
  public class HATEOASAuthorFilterAttribute:HATEOASFilterAttribute
  {
    private readonly LinksGeneratorService linksGeneratorService;

    public HATEOASAuthorFilterAttribute(LinksGeneratorService linksGeneratorService) {
      this.linksGeneratorService = linksGeneratorService;
    }

    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
      bool mustIncludeHATEOAS = MustIncludeHATEOAS(context);

      if (!mustIncludeHATEOAS)
      {
        await next();
        return;
      }
      
      var result = context.Result as ObjectResult;

      var authorWithBooksDTO = result.Value as AuthorWithBooksDTO;

      if (authorWithBooksDTO is null)
      {
        List<AuthorWithBooksDTO> authorWithBooksDTOList = result.Value as List<AuthorWithBooksDTO> ??
        throw new ArgumentNullException("An instance of AuthorWithBooksDTO or List<AuthorWithBooksDTO> was expected");

        authorWithBooksDTOList.ForEach(async authorDTO => await linksGeneratorService.GenerateLinksAuthor(authorDTO));

        result.Value = authorWithBooksDTOList;
      }
      else
      {
        await linksGeneratorService.GenerateLinksAuthor(authorWithBooksDTO);
      }

      await next();

    }
  }
}
