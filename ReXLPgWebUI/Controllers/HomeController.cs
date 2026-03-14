using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace ReXLPgWebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StoreToken([FromBody] string token)
        {
            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });

            return Ok();
        }
        [HttpGet]
        public IActionResult RemoveToken()
        {
            Response.Cookies.Delete("jwt");
            return Ok();
        }
    }
}
