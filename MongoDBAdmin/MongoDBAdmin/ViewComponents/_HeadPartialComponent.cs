using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.ViewComponents
{
    public class _HeadPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
