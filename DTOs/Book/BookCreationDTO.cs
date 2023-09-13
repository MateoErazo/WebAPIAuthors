using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.Validations;

namespace WebAPIAuthors.DTOs.Book
{
  public class BookCreationDTO
  {

    [Required]
    [FirstLetterCapitalized]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<int> AuthorsIds { get; set; }
  }
}
