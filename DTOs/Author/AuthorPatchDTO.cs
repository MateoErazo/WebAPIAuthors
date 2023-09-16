using System.ComponentModel.DataAnnotations;
using WebAPIAuthors.Validations;

namespace WebAPIAuthors.DTOs.Author
{
  public class AuthorPatchDTO
  {
    public string Name { get; set; }

    public int Age { get; set; }
  }
}
