using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.DTOs.Book;
using WebAPIAuthors.Validations;

namespace WebAPIAuthors.DTOs.Author
{
  public class AuthorWithBooksDTO
  {
    public int Id { get; set; }

    [Required]
    [FirstLetterCapitalized]
    public string Name { get; set; }

    public int Age { get; set; }

    public List<BookGetDTO> Books { get; set; }
  }
}
