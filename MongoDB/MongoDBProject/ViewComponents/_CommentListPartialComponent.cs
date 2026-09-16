using System.Threading.Tasks;
using Application.Features.Comment.Queries.GetList;
using Application.PageResult;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.ViewComponents
{
    public class _CommentListPartialComponent : _BaseComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int size = 30, int index = 1)
        {
            var paged = new PageResult(size, index);
            var list = await Mediator.Send(new GetListCommentQuery{ pageResult = paged });
            return View(list);
        }
        
    }
}
