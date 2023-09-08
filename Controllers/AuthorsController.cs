using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAuthors.Entities;

namespace WebAPIAuthors.Controllers
{

  [ApiController]
  [Route("api/authors")]
  public class AuthorsController:ControllerBase
  {
    private readonly ApplicationDbContext context;

    public AuthorsController(ApplicationDbContext context) 
    {
      this.context = context;
    }


    /// <summary>
    /// Search all authors in database
    /// </summary>
    /// <returns>The list of all authors in database</returns>
    [HttpGet("all",Name ="getAllAuthors")]
    public async Task<ActionResult<List<Author>>> GetAllAuthors()
    {
      List<Author> authors = await context.Authors.ToListAsync();
      return authors;
    }

    /// <summary>
    /// Search one author that match with the id received.
    /// </summary>
    /// <param name="id">The unique Id of the author</param>
    /// <returns>An Author identity. If It's not found, return not found</returns>
    [HttpGet("{id:int}",Name ="getSingleAuthorById")]
    public async Task<ActionResult<Author>> GetSingleAuthorById(int id)
    {
      Author author = await context.Authors.FirstOrDefaultAsync(x=>x.Id == id);

      if (author is null)
      {
        return NotFound($"There is not an author with id {id}.Please check your Id and try again.");
      }

      return author;
    }

    /// <summary>
    /// Add a new author to database.
    /// </summary>
    /// <param name="author">The Author object with the data to create</param>
    /// <returns>The location of the new author.</returns>
    [HttpPost(Name = "addNewAuthor")]
    public async Task<ActionResult> AddNewAuthor(Author author)
    {
      context.Add(author);
      await context.SaveChangesAsync();
      return CreatedAtRoute("getSingleAuthorById", new {id=author.Id}, author);
    }

    /// <summary>
    /// It makes a complete update with an Author existing
    /// </summary>
    /// <param name="id">The unique Id Author that would be updated</param>
    /// <param name="author">The Author object with the data to update</param>
    /// <returns>No content. If the author not exist, return not found.</returns>
    [HttpPut("{id:int}",Name ="updateCompleteAuthor")]
    public async Task<ActionResult> UpdateCompleteAuthor(int id, Author author)
    {
      bool existAuthor = await context.Authors.AnyAsync(x => x.Id == id);

      if (!existAuthor)
      {
        return NotFound($"There is not an author with id {id}.Please check your Id and try again.");
      }

      context.Update(author);
      await context.SaveChangesAsync();
      return NoContent();
    }

    /// <summary>
    /// Delete an author by It's Id
    /// </summary>
    /// <param name="id">The unique Id Author that would be deleted</param>
    /// <returns>No content. If author not exist, return not found</returns>
    [HttpDelete("{id:int}")]
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

  }
}
