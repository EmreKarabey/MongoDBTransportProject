using Application.Features.Brand.Queries.GetActive;
using Application.Features.GetInTouch.Queries.GetList;
using Application.PageResult;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _GetInTouchPartialComponent:_BaseComponent
    {
        public async Task< IViewComponentResult> InvokeAsync(int size = 100,int index=1)
        {
            var paged = new PageResult(size, index);

            var list = await Mediator.Send(new GetListGetInTouchQuery { pageResult = paged });

            return View(list);
        }
    }
}
