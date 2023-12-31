using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Web_153505_Shevtsova_D.Controllers
{
    public class IdentityController : Controller
    {
        // а точно ли пост?
        public async Task Login()
        {
            await HttpContext.ChallengeAsync(
                "oidc",
                new AuthenticationProperties
                {
                    RedirectUri =
                        Url.Action("Index", "Home")
                });
        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync("cookie");   
            await HttpContext.SignOutAsync("oidc",
                new AuthenticationProperties
                {
                    RedirectUri =
                        Url.Action("Index", "Home")
                });
        }

    }
}

