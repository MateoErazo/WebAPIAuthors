using System.ComponentModel.DataAnnotations;

namespace WebAPIAuthors.DTOs.Accounts
{
  public class EditUserDTO
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  }
}
