using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Comment.Command.Create;
using Application.Services.Helsinki;
using Application.Services.ToxicBert;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SharpCompress.Common;

namespace MongoDBProject.Controllers
{
    public class CommentController : BaseController
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHelsinkiService _helsinkiService;
        private readonly IToxicBertService _toxicBertService;

        public CommentController(UserManager<AppUser> userManager, IMapper mapper, IHelsinkiService helsinkiService, IToxicBertService toxicBertService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _helsinkiService = helsinkiService;
            _toxicBertService = toxicBertService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(CreateCommentCommand command)
        {
            var userId = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId != null)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    command.AppUserId = user.Id;
                    if (user.ImageURL != null) command.ImageURL = user.ImageURL;
                    command.FullName = user.FullName;

                    var commentSubject = _helsinkiService.Translate(command.Content);
                    var commentDetails = _helsinkiService.Translate(command.Details);

                    var taskAll = await Task.WhenAll(commentSubject,commentDetails);

                    var EnSubject = commentSubject.Result;
                    var EnDetails = commentDetails.Result;


                    var textscore1 = _toxicBertService.ToxicScore(EnSubject);
                    var textscore2 = _toxicBertService.ToxicScore(EnDetails);

                    var taskAll2 = await Task.WhenAll(textscore1, textscore2);

                    var TextScore1 = textscore1.Result;
                    var TextScore2 = textscore2.Result;

                    bool IsToxic(List<ToxicScoreDto> toxicScoreDtos) => toxicScoreDtos.Any(s => s.Score > 0.5);

                    if (IsToxic(TextScore1) || IsToxic(TextScore2))
                    {
                        TempData["ErrorMessage"] = "Yorumunuz topluluk kurallarımıza uymayan ifadeler içerdiği için reddedildi. Lütfen yorumunuzu düzenleyip tekrar deneyin.";
                        return RedirectToAction("Index");
                    }

                    await Mediator.Send(command);



                    TempData["SuccessMessage"] = "Yorumunuz başarıyla gönderildi. Teşekkür ederiz!";
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "Yorum yapabilmek için giriş yapmalısınız.";
            return View();
        }
    }
}
