using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAuthors.DTOs.Accounts
{
  public class UserCredentialsDTO
  {
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
  }
}
