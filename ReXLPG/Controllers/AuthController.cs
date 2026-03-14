using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReXLPgAPI.Models;
using ReXLPgDAS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReXLPgAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IUserService _userService;
        public AuthController(IUserService user)
        {
            _userService = user;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = new ReXLPgDM.User() { Mobile = model.Mobile, Password = model.Password };
            if (_userService.TryFeatchDetailForLogin(ref user))
            {
                var claims = new[]{
                                        new Claim("UserId", user.GUID.ToString()),
                                        new Claim(ClaimTypes.Name, user.Name),
                                        new Claim(ClaimTypes.GivenName, user.DisplayName),
                                        new Claim(ClaimTypes.Role, user.Designation.ToString()),
                                        new Claim(ClaimTypes.MobilePhone, user.Mobile.ToString())
                                    };
                var token = GenerateJwtToken(claims);


                return new JsonResult(new ActionResponse() { SucessValue = token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(Claim[] claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("M0MjGA3c4w6FhXpavzFwOuDchrBo9JSZ"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Retinue",
                audience: "ReXLPG",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
