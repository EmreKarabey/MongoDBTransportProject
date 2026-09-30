using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.ViewComponents
{
    public class _MembersListPartialComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public _MembersListPartialComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var list = await _userManager.GetUsersInRoleAsync("Üye");
            return View(list);
        }
    }
}
