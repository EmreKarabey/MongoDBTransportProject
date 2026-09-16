using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _NavbarPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
