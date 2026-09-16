using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.ViewComponents
{
    public class _FooterPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
