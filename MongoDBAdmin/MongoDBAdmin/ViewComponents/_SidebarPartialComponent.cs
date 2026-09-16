using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.ViewComponents
{
    public class _SidebarPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
