namespace WebAPIAuthors.DTOs.Accounts
{
  public class AuthenticationResultDTO
  {
    public string Token { get; set; }

    public DateTime Expiration { get; set; }
    
  }
}
