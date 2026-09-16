using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _FooterPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
