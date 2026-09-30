using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Shipment.Queries.GetFiltreStatus;
using Application.Features.Shipment.Queries.GetUserShipment;
using Application.PageResult;
using Microsoft.AspNetCore.Mvc;
using Persistence.Paginate;

namespace MongoDBProject.Controllers
{
    public class ShipmentController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> Index(int index = 1, int size = 30)
        {
            var userClaim = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null)
                return RedirectToAction("Login", "Account");

            var userId = Guid.Parse(userClaim.Value);
            var paged = new PageResult(size, index);

            var list = await Mediator.Send(new GetListUserShipmentQuery
            {
                pageResult = paged,
                AppUserId = userId
            });
            return View(list);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> StatusFiltre(string? filtre, int index = 1, int size = 30)
        {
            var userClaim = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userClaim == null)
                return RedirectToAction("Login", "Account");

            var userId = Guid.Parse(userClaim.Value);
            var paged = new PageResult(size, index);

            var query = new GetListUserShipmentQuery
            {
                pageResult = paged,
                AppUserId = userId,
                Status = (filtre == "all") ? null : filtre
            };

            var list = await Mediator.Send(query);

            return View("Index", list);
        }
    }
}
