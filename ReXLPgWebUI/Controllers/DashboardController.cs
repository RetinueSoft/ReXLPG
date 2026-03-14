using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;

namespace ReXLPgWebUI.Controllers
{
    [Authorize(AuthenticationSchemes = "JwtCookieScheme")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            //var userImage = User.FindFirst("UserImage")?.Value;
            //var userName = User.Identity?.Name;
            //var displayName = User.FindFirst(ClaimTypes.GivenName)?.Value;
            //var mobile = User.FindFirst(ClaimTypes.MobilePhone)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            return userRole.ToUpper() switch
            {
                "SUPERADMIN" or "ADMIN" or "BUSINESSOWNER" => Admin(),
                "SUPERVISOR" => Supervisor(),
                "REGULARCUSTOMER" or "WALKINCUSTOMER" => Tenant(),
                _ => RedirectToAction("Index", "Home")
            };
        }

        public IActionResult Admin()
        {
            return View("Admin");
        }
        public IActionResult Supervisor()
        {
            return View("Supervisor");
        }
        public IActionResult Tenant()
        {
            return View("Tenant");
        }
    }
}
