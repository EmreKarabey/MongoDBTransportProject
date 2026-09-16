using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.ViewComponents
{
    public class _ScriptPartialComponent:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
