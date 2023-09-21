using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAuthors.DTOs.Author;
using WebAPIAuthors.DTOs.HATEOAS;
using WebAPIAuthors.Entities;
using WebAPIAuthors.Services;

namespace WebAPIAuthors.Controllers
{

  [ApiController]
  [Route("api/authors")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class AuthorsController:ControllerBase
  {
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IAuthorizationService authorizationService;

    public AuthorsController(
      ApplicationDbContext context,
      IMapper mapper,
      IAuthorizationService authorizationService) 
    {
      this.context = context;
      this.mapper = mapper;
      this.authorizationService = authorizationService;
    }


    /// <summary>
    /// Search all authors in database
    /// </summary>
    /// <returns>The list of all authors in database</returns>
    [HttpGet("all",Name ="getAllAuthors")]
    public async Task<ActionResult> GetAllAuthors([FromQuery] bool includeHATEOAS = true)
    {
      List<Author> authors = await context.Authors
        .Include(x=>x.AuthorBooks)
        .ThenInclude(x=>x.Book)
        .ToListAsync();
      var authorsDTO = mapper.Map<List<AuthorWithBooksDTO>>(authors);

      if (includeHATEOAS)
      {
        authorsDTO.ForEach(async authorDTO => await GenerateLinksAuthor(authorDTO));

        ResourceCollection<AuthorWithBooksDTO> result = new ResourceCollection<AuthorWithBooksDTO>
        {
          Values = authorsDTO
        };

        result.Resources.Add(new DataHATEOAS(
          link: Url.Link("getAllAuthors", new { }),
          description: "self",
          method: "GET"));

        var isAdmin = await authorizationService.AuthorizeAsync(User, "isAdmin");

        if (isAdmin.Succeeded)
        {
          result.Resources.Add(new DataHATEOAS(
          link: Url.Link("addNewAuthor", new { }),
          description: "add-new-author",
          method: "POST"));
        }

        return Ok(result);

      }

      return Ok(authorsDTO);
    }

    /// <summary>
    /// Search one author that match with the id received.
    /// </summary>
    /// <param name="id">The unique Id of the author</param>
    /// <returns>An Author identity. If It's not found, return not found</returns>
    [HttpGet("{id:int}",Name ="getSingleAuthorById")]
    public async Task<ActionResult<AuthorWithBooksDTO>> GetSingleAuthorById(int id)
    {
      Author author = await context.Authors
        .Include(x=>x.AuthorBooks)
        .ThenInclude(x =>x.Book)
        .FirstOrDefaultAsync(x=>x.Id == id);

      if (author is null)
      {
        return NotFound($"There is not an author with id {id}.Please check your Id and try again.");
      }

      AuthorWithBooksDTO authorWithBooksDTO = mapper.Map<AuthorWithBooksDTO>(author);

      await GenerateLinksAuthor(authorWithBooksDTO);

      return authorWithBooksDTO;
    }

    /// <summary>
    /// Add a new author to database.
    /// </summary>
    /// <param name="author">The Author object with the data to create</param>
    /// <returns>The location of the new author.</returns>

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="isAdmin")]
    [HttpPost(Name = "addNewAuthor")]
    public async Task<ActionResult> AddNewAuthor(AuthorCreationDTO authorCreationDTO)
    {
      Author author = mapper.Map<Author>(authorCreationDTO);
      context.Add(author);
      await context.SaveChangesAsync();

      AuthorGetDTO authorDTO = mapper.Map<AuthorGetDTO>(author);
      return CreatedAtRoute("getSingleAuthorById", new {id=author.Id}, authorDTO);
    }

    /// <summary>
    /// It makes a complete update with an Author existing
    /// </summary>
    /// <param name="id">The unique Id Author that would be updated</param>
    /// <param name="author">The Author object with the data to update</param>
    /// <returns>No content. If the author not exist, return not found.</returns>
  
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy ="isAdmin")]
    [HttpPut("{id:int}",Name ="updateCompleteAuthor")]
    public async Task<ActionResult> UpdateCompleteAuthor(int id, AuthorCreationDTO authorCreationDTO)
    {
      Author author = await context.Authors.FirstOrDefaultAsync(x => x.Id == id);

      if (author is null)
      {
        return NotFound($"There is not an author with id {id}.Please check your Id and try again.");
      }

      author = mapper.Map(authorCreationDTO,author);
      
      await context.SaveChangesAsync();
      return NoContent();
    }

    /// <summary>
    /// Delete an author by It's Id
    /// </summary>
    /// <param name="id">The unique Id Author that would be deleted</param>
    /// <returns>No content. If author not exist, return not found</returns>

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    [HttpDelete("{id:int}", Name ="deleteAuthor")]
    public async Task<ActionResult> DeleteAuthor(int id)
    {
      bool existAuthor = await context.Authors.AnyAsync(x => x.Id == id);

      if (!existAuthor)
      {
        return NotFound($"There is not an author with id {id}.Please check your Id and try again.");
      }

      context.Remove(new Author { Id = id});
      await context.SaveChangesAsync();
      return NoContent();
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "isAdmin")]
    [HttpPatch("{id:int}",Name ="updatePartialAuthor")]
    public async Task<ActionResult> UpdatePartialAuthor(int id, JsonPatchDocument<AuthorPatchDTO> jsonPatchDocument)
    {
      if (jsonPatchDocument is null)
      {
        return BadRequest();
      }

      Author author = await context.Authors.FirstOrDefaultAsync(x=>x.Id == id);

      if (author is null)
      {
        return NotFound($"There is not an author with id {id}");
      }

      AuthorPatchDTO authorPatchDTO = mapper.Map<AuthorPatchDTO>(author);
      jsonPatchDocument.ApplyTo(authorPatchDTO, ModelState);

      bool isModelValid = TryValidateModel(authorPatchDTO);

      if (!isModelValid)
      {
        return BadRequest(ModelState);
      }

      mapper.Map(authorPatchDTO,author);
      await context.SaveChangesAsync();
      return NoContent();

    }


    private async Task GenerateLinksAuthor(AuthorWithBooksDTO authorWithBooksDTO)
    {
      List<DataHATEOAS> resourcesAuthor = new List<DataHATEOAS>();

      bool isAuthenticated = User.Identity.IsAuthenticated;
      var isAdmin = await authorizationService.AuthorizeAsync(User,"isAdmin");

      if (isAuthenticated)
      {
        resourcesAuthor.Add(new DataHATEOAS(link: Url.Link("getSingleAuthorById", new { id = authorWithBooksDTO.Id }),
        description: "self",
        method: "GET"));
      }

      if (isAdmin.Succeeded)
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
