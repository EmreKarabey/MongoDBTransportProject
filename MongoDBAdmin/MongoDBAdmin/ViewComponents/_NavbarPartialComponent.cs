using System.Threading.Tasks;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.ViewComponents
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
            if (UserClaimsPrincipal.Identity != null && UserClaimsPrincipal.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
                ViewBag.DisplayName = !string.IsNullOrWhiteSpace(user?.FullName) ? user.FullName : UserClaimsPrincipal.Identity.Name;
                ViewBag.UserImage = !string.IsNullOrWhiteSpace(user?.ImageURL) ? user.ImageURL : "/Silva-Admin/dist/assets/images/users/user-5.jpg";
            }
            else
            {
                ViewBag.DisplayName = "Yönetici";
                ViewBag.UserImage = "/Silva-Admin/dist/assets/images/users/user-5.jpg";
            }

            return View();
        }
    }
}
