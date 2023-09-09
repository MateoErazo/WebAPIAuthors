using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.Validations;

namespace WebAPIAuthors.Entities
{
  public class Author
  { 
    public int Id { get; set; }

    [Required]
    [FirstLetterCapitalized]
    public string Name { get; set; }

    public int Age { get; set; }
  }
}
