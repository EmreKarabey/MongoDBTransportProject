using System.Threading.Tasks;
using BusinessLayer.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBAdmin.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _commentService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Yorum listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteComment(string Id)
        {
            try
            {
                var entity = await _commentService.GetByIdAsync(Id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Silinecek yorum bulunamadı. Kayıt zaten silinmiş olabilir." });
                }

                await _commentService.DeleteAsync(entity.CommentId);
                return Json(new { success = true, message = "Yorum başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Yorum silinirken bir hata oluştu: {ex.Message}" });
            }
        }
    }
}
