using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAuthors.Entities;

namespace WebAPIAuthors.Controllers
{

  [ApiController]
  [Route("api/books")]
  public class BooksController:ControllerBase
  {
    private readonly ApplicationDbContext context;

    public BooksController(ApplicationDbContext context) 
    {
      this.context = context;
    }


    /// <summary>
    /// Search all books in database
    /// </summary>
    /// <returns>The list of all book in database</returns>
    [HttpGet("all", Name = "getAllBooks")]
    public async Task<ActionResult<List<Book>>> GetAllBooks()
    {
      List<Book> books = await context.Books.ToListAsync();
      return books;
    }

    /// <summary>
    /// Search one book that match with the id received.
    /// </summary>
    /// <param name="id">The unique Id of the book</param>
    /// <returns>An Book identity. If It's not found, return not found</returns>
    [HttpGet("{id:int}", Name = "getSingleBookById")]
    public async Task<ActionResult<Book>> GetSingleBookById(int id)
    {
      Book book = await context.Books.FirstOrDefaultAsync(x => x.Id == id);

      if (book is null)
      {
        return NotFound($"There is not an book with id {id}.Please check your Id and try again.");
      }

      return book;
    }

    /// <summary>
    /// Add a new book to database.
    /// </summary>
    /// <param name="author">The Book object with the data to create</param>
    /// <returns>The location of the new book.</returns>
    [HttpPost(Name = "addNewBook")]
    public async Task<ActionResult> AddNewBook(Book book)
    {
      bool existAuthor = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

      if (!existAuthor)
      {
        return NotFound($"There is not an author with id {book.AuthorId}.Please check your Id and try again.");
      }

      context.Add(book);
      await context.SaveChangesAsync();
      return CreatedAtRoute("getSingleBookById", new { id = book.Id }, book);
    }

    /// <summary>
    /// It makes a complete update with an Book existing
    /// </summary>
    /// <param name="id">The unique Id Book that would be updated</param>
    /// <param name="author">The Book object with the data to update</param>
    /// <returns>No content. If the book not exist, return not found.</returns>
    [HttpPut("{id:int}", Name = "updateCompleteBook")]
    public async Task<ActionResult> UpdateCompleteBook(int id, Book book)
    {
      bool existBook = await context.Books.AnyAsync(x => x.Id == id);

      if (!existBook)
      {
        return NotFound($"There is not an book with id {id}.Please check your Id and try again.");
      }

      bool existAuthor = await context.Authors.AnyAsync(x => x.Id == book.AuthorId);

      if (!existAuthor)
      {
        return NotFound($"There is not an author with id {book.AuthorId}.Please check your Id and try again.");
      }

      context.Update(book);
      await context.SaveChangesAsync();
      return NoContent();
    }

    /// <summary>
    /// Delete an book by It's Id
    /// </summary>
    /// <param name="id">The unique Id Book that would be deleted</param>
    /// <returns>No content. If book not exist, return not found</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBook(int id)
    {
      bool existBook = await context.Books.AnyAsync(x => x.Id == id);

      if (!existBook)
      {
        return NotFound($"There is not an book with id {id}.Please check your Id and try again.");
      }

      context.Remove(new Book { Id = id });
      await context.SaveChangesAsync();
      return NoContent();
    }
  }
}
