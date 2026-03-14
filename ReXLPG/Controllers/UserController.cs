using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReXL.Util;
using ReXLPgAPI.Models;
using ReXLPgDA;
using ReXLPgDAS;
using ReXLPgDM;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReXLPgAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IUserDA _userDA;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public UserController(IUserService user, IUserDA userDA, IConfiguration config, IWebHostEnvironment hostingEnvironment)
        {
            _userService = user;
            _userDA = userDA;
            _config = config;
            _hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserCreateModel model)
        {
            var user = model.ConvertToObject<User>();
            user.RegisterDate = DateTime.UtcNow;
            user.Designation = UserRole.WalkinCustomer;
            user.DisplayName = model.Name;
            user.Gender = Gender.Female;

            var whatsappIntegrated = _config.GetValue<bool>("AppSettings:WhatsappIntegrated");
            _userService.Add(user, whatsappIntegrated, model.EnquirForMonths);

            return new JsonResult(new ActionResponse());
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] UserEditModel model)
        {
            _userService.EditProfile(model.ConvertToObject<User>());
            return new JsonResult(new ActionResponse());
        }

        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword(Guid userId, string password)
        {
            _userService.UpdatePassword(userId, password);
            return new JsonResult(new ActionResponse());
        }

        [HttpPost("UploadDocument")]
        public async Task<IActionResult> UploadDocument([FromBody] UserDocumentModel model)
        {
            var allowedSizeKB = _config.GetValue<int>("AppSettings:AllowedSmallImageInKB");
            model.UserImage = await ImageHelper.ResizeImageToTargetSizeAsync(model.UserImage, allowedSizeKB/2);
            model.AadharFrontImage = await ImageHelper.ResizeImageToTargetSizeAsync(model.AadharFrontImage, allowedSizeKB);
            model.AadharBackImage = await ImageHelper.ResizeImageToTargetSizeAsync(model.AadharBackImage, allowedSizeKB);
            model.WorkIdFrontImage = await ImageHelper.ResizeImageToTargetSizeAsync(model.WorkIdFrontImage, allowedSizeKB);
            model.WorkIdBackImage = await ImageHelper.ResizeImageToTargetSizeAsync(model.WorkIdBackImage, allowedSizeKB);

            _userService.UpdateDocument(model.ConvertToObject<UserDocument>());
            return new JsonResult(new ActionResponse());
        }

        [HttpGet("LoginUserImage")]
        public IActionResult GetLoggedinUserImage()
        {
            var userid = User.FindFirst("UserId")?.Value;
            if (!Guid.TryParse(userid, out var guid))
                throw new Exception("Userid not found, relogin and try again");

            var filePath = Directory.GetCurrentDirectory() + "\\wwwroot" + _config["AppSettings:DummyUserImagePath"];
            if (!System.IO.File.Exists(filePath))
                throw new Exception("User Image not found");

            var dummyImage = System.IO.File.ReadAllBytes(filePath);
            var documents = _userDA.GetDocuments(guid);
            if (documents == null)
                documents = new UserDocumentModel();
            if (documents.UserImage.IsNullOrEmpty())
                documents.UserImage = dummyImage;
            return new JsonResult(new ActionResponse() { SucessValue = documents.UserImage });

            //return new JsonResult(new ActionResponse() { SucessValue = Convert.ToBase64String(documents.UserImage) });
        }

        [HttpGet("MyDocumentImages")]
        public IActionResult GetLoggedinDocuments()
        {
            var userid = User.FindFirst("UserId")?.Value;
            if (!Guid.TryParse(userid, out var guid))
                throw new Exception("Userid not found, relogin and try again");

            var dummyImagePath = Directory.GetCurrentDirectory() + "\\wwwroot" + _config["AppSettings:DummyImagePath"];
            if (!System.IO.File.Exists(dummyImagePath))
                throw new Exception("User Aadhar Front Image not found");

            var dummyImage = System.IO.File.ReadAllBytes(dummyImagePath);
            var documents = _userDA.GetDocuments(guid);
            if (documents == null)
                documents = new UserDocumentModel();
            if (documents.AadharFrontImage.IsNullOrEmpty())
                documents.AadharFrontImage = dummyImage;
            if (documents.AadharBackImage.IsNullOrEmpty())
                documents.AadharBackImage = dummyImage;
            if (documents.WorkIdFrontImage.IsNullOrEmpty())
                documents.WorkIdFrontImage = dummyImage;
            if (documents.WorkIdBackImage.IsNullOrEmpty())
                documents.WorkIdBackImage = dummyImage;

            return new JsonResult(new ActionResponse() { SucessValue = documents });
            //var responce = new ActionResponse()
            //{
            //    SucessValue = new
            //    {
            //        AadharFrontImage = Convert.ToBase64String(aadharFrontImage),
            //        AadharBackImage = Convert.ToBase64String(aadharBackImage),
            //        WorkIdImage = Convert.ToBase64String(workIdImage),
            //    }
            //};

            //return new JsonResult(responce);
        }

        [HttpGet("MyDetail")]
        public IActionResult GetLoggedinDetail()
        {
            var userid = User.FindFirst("UserId")?.Value;
            if (!Guid.TryParse(userid, out var guid))
                throw new Exception("Userid not found, relogin and try again");
            return new JsonResult(new ActionResponse() { SucessValue = _userDA.Get(guid) });
        }

        [HttpGet("MyShortDetail")]
        public IActionResult GetShortLoggedinDetail()
        {
            var userName = User.Identity?.Name;
            var displayName = User.FindFirst(ClaimTypes.GivenName)?.Value;
            var mobile = User.FindFirst(ClaimTypes.MobilePhone)?.Value;

            var user = new UserCreateModel() { Name =  displayName, Mobile = mobile };
            return new JsonResult(new ActionResponse() { SucessValue = user });
        }
    }
}
