using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _MobileNavbarPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
