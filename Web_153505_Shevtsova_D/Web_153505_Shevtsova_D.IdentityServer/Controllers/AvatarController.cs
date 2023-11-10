using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Web_153505_Shevtsova_D.IdentityServer.Models;

namespace Web_153505_Shevtsova_D.IdentityServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AvatarController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly UserManager<ApplicationUser> _userManager;
        public AvatarController(IWebHostEnvironment environment, UserManager<ApplicationUser> userManager)
        {
            _environment = environment;
            _userManager = userManager;
        }
        public IActionResult GetFileAvatar()
        {
            var id = _userManager.GetUserId(User);
            var avatarPath = Directory.GetFiles(Path.Combine(_environment.ContentRootPath, "Images"), $"{id}.*").FirstOrDefault();

            string mimeType = "";
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();

            if (avatarPath != null && System.IO.File.Exists(avatarPath))
            {
                provider.TryGetContentType(avatarPath, out mimeType);

                using var stream = System.IO.File.OpenRead(avatarPath);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return new FileContentResult(data, mimeType);
            }
            else
            {
                var profileIconPath = Path.Combine(_environment.ContentRootPath, "Images", "kot.png");
                provider.TryGetContentType(profileIconPath, out mimeType);

                using var stream = System.IO.File.OpenRead(profileIconPath);
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);
                return new FileContentResult(data, mimeType);
            }
        }
    }
}

