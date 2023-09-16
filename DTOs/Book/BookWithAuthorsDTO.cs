using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.DTOs.Author;
using WebAPIAuthors.Entities;

namespace WebAPIAuthors.DTOs.Book
{
  public class BookWithAuthorsDTO
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<AuthorGetDTO> Authors { get; set; }
  }
}
