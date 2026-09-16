using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _HeadPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }

}
