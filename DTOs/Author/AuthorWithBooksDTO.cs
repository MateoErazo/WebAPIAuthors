using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.DTOs.Book;
using WebAPIAuthors.DTOs.HATEOAS;
using WebAPIAuthors.Validations;

namespace WebAPIAuthors.DTOs.Author
{
  public class AuthorWithBooksDTO:Resource
  {
    public int Id { get; set; }

    [Required]
    [FirstLetterCapitalized]
    public string Name { get; set; }

    public int Age { get; set; }

    public List<BookGetDTO> Books { get; set; }
  }
}
