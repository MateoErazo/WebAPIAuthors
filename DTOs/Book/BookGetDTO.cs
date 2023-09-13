using System.ComponentModel.DataAnnotations;

namespace WebAPIAuthors.DTOs.Book
{
  public class BookGetDTO
  {
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
  }
}
