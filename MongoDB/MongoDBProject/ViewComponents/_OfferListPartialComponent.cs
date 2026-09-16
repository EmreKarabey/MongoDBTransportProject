using System.Threading.Tasks;
using Application.Features.Offer.Queries.GetActiveList;
using Application.PageResult;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _OfferListPartialComponent:_BaseComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int size = 30, int index = 1)
        {
            var paged = new PageResult(size, index);

            var list = await Mediator.Send(new GetActiveOfferListQuery{ pageResult=paged});
            return View(list);
        }
    }
}
