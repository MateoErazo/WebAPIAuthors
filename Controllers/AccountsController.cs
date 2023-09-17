using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAuthors.DTOs.Accounts;

namespace WebAPIAuthors.Controllers
{
  [ApiController]
  [Route("api/accounts")]
  public class AccountsController:ControllerBase
  {
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly IConfiguration configuration;

    public AccountsController(
      UserManager<IdentityUser> userManager,
      SignInManager<IdentityUser> signInManager,
      IConfiguration configuration) 
    {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthenticationResultDTO>> AddNewUser(UserCredentialsDTO userCredentialsDTO)
    {
      IdentityUser identityUser = new IdentityUser { UserName = userCredentialsDTO.Email, Email = userCredentialsDTO.Email };
      var identityResult = await userManager.CreateAsync(identityUser, userCredentialsDTO.Password);

      if (identityResult.Succeeded)
      {
        return await CreateToken(userCredentialsDTO);
      }
      else
      {
        return BadRequest(identityResult.Errors);
      }        
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResultDTO>> LoginUser(UserCredentialsDTO userCredentialsDTO)
    {
      var loginResult = await signInManager.PasswordSignInAsync(userCredentialsDTO.Email, userCredentialsDTO.Password, false, false);

      if (loginResult.Succeeded)
      {
        return await CreateToken(userCredentialsDTO);
      }
      else
      {
        return BadRequest("Wrong login");
      }
    }

    private async Task<AuthenticationResultDTO> CreateToken(UserCredentialsDTO userCredentialsDTO)
    {
      List<Claim> claims = new List<Claim>
      {
        new Claim("email", userCredentialsDTO.Email),
        new Claim("anything key","anything value")
      };

      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));

      var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

      DateTime expiration = DateTime.UtcNow.AddMinutes(30);

      var securityToken = new JwtSecurityToken(
        issuer:null, audience: null, claims:claims, expires:expiration, signingCredentials:credentials);

      return new AuthenticationResultDTO()
      {
        Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
        Expiration = expiration,
      };
    
    }


  }
}
