using System.Diagnostics;
using System.Threading.Tasks;
using Application.Features.Slider.Queries.GetList;
using Application.PageResult;
using Microsoft.AspNetCore.Mvc;
using MongoDBProject.Models;
using Persistence.Paginate;

namespace MongoDBProject.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(int size = 30, int index = 1)
        {
            var paged = new PageResult { Index = index ,Size=size};
            var list = await Mediator.Send(new GetListSliderQuery { pageResult =paged});
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
