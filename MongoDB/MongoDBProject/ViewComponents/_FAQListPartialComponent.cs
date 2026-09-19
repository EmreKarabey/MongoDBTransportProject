using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.PageResult;
using Application.Features.FAQ.Queries.GetActiveList;
namespace MongoDBProject.ViewComponents
{
    public class _FAQListPartialComponent : _BaseComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int size = 30, int index = 1)
        {
            var paged = new PageResult(size, index);
            var list = await Mediator.Send(new GetActiveListFAQQuery { pageResult = paged });
            return View(list);
        }
    }
}
