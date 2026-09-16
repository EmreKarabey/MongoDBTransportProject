using System.Threading.Tasks;
using Application.Features.Brand.Queries.GetActive;
using Application.Features.Brand.Queries.GetList;
using Application.PageResult;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _BrandListPartialComponent : _BaseComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int size = 30, int index = 1)
        {
            var paged = new PageResult(size,index);

            var list = await Mediator.Send(new GetActiveListBrandQuery { pageResult = paged });

            return View(list);
        }
    }
}
