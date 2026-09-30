using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _StaticsPartialComponent:_BaseComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
