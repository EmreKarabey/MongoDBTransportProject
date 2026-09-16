using Application.Features.About.Queries.GetActive;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _AboutPartialComponent:_BaseComponent
    {
        public async Task< IViewComponentResult> InvokeAsync()
        {
            var active =await Mediator.Send(new GetActiveAboutQuery());
            return View(active);
        }
    }
}
