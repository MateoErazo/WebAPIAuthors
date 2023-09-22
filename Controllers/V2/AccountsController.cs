using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAuthors.DTOs.Accounts;

namespace WebAPIAuthors.Controllers.V2
{
    [ApiController]
    [Route("api/v2/accounts")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountsController : ControllerBase
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
        [AllowAnonymous]
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
        [AllowAnonymous]
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

        [HttpGet("renewToken")]
        public async Task<ActionResult<AuthenticationResultDTO>> RenewToken()
        {
            Claim userClaim = HttpContext.User.Claims.Where(x => x.Type == "email").FirstOrDefault();
            return await CreateToken(new UserCredentialsDTO { Email = userClaim.Value });
        }

        [HttpPost("doAdmin")]
        public async Task<ActionResult> DoAdmin(EditUserDTO editUserDTO)
        {
            IdentityUser identityUser = await userManager.FindByEmailAsync(editUserDTO.Email);
            await userManager.AddClaimAsync(identityUser, new Claim("isAdmin", "1"));
            return NoContent();
        }

        [HttpPost("removeAdmin")]
        public async Task<ActionResult> RemoveAdmin(EditUserDTO editUserDTO)
        {
            IdentityUser identityUser = await userManager.FindByEmailAsync(editUserDTO.Email);
            await userManager.RemoveClaimAsync(identityUser, new Claim("isAdmin", "1"));
            return NoContent();
        }

        private async Task<AuthenticationResultDTO> CreateToken(UserCredentialsDTO userCredentialsDTO)
        {
            List<Claim> claims = new List<Claim>
      {
        new Claim("email", userCredentialsDTO.Email),
        new Claim("anything key","anything value")
      };

            IdentityUser identityUser = await userManager.FindByEmailAsync(userCredentialsDTO.Email);
            var claimsUser = await userManager.GetClaimsAsync(identityUser);
            claims.AddRange(claimsUser);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(
              issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            return new AuthenticationResultDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration,
            };

        }


    }
}
