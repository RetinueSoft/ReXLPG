using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace ReXLPgWebUI.Controllers
{
    [Authorize(AuthenticationSchemes = "JwtCookieScheme")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
