using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _NavbarPartialComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _NavbarPartialComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (UserClaimsPrincipal?.Identity != null && UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
                ViewBag.IsAuthenticated = true;
                ViewBag.DisplayName = !string.IsNullOrWhiteSpace(user?.FullName) ? user.FullName : UserClaimsPrincipal.Identity.Name;
                ViewBag.UserImage = !string.IsNullOrWhiteSpace(user?.ImageURL) ? user.ImageURL : null;
                ViewBag.UserName = user?.UserName;
            }
            else
            {
                ViewBag.IsAuthenticated = false;
                ViewBag.DisplayName = null;
                ViewBag.UserImage = null;
                ViewBag.UserName = null;
            }

            return View();
        }
    }
}
