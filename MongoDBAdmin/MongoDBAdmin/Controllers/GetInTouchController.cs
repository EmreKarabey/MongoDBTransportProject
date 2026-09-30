using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.GetInTouch;

namespace MongoDBAdmin.Controllers
{
    [Authorize(Roles = "Admin,Moderatör")]
    [AutoValidateAntiforgeryToken]
    public class GetInTouchController : Controller
    {
        private readonly IGetInTouchService _getInTouchService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public GetInTouchController(IGetInTouchService getInTouchService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _getInTouchService = getInTouchService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _getInTouchService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"İletişim bilgileri yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddGetInTouch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddGetInTouch(CreateGetInTouchDto createGetInTouchDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları doldurun.";
                return View(createGetInTouchDto);
            }

            try
            {
                if (createGetInTouchDto.ImageFile != null && createGetInTouchDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(createGetInTouchDto.ImageFile, "getintouch");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(createGetInTouchDto);
                    }
                    createGetInTouchDto.ImageURL = imageUrl;
                }

                var entity = _mapper.Map<GetInTouch>(createGetInTouchDto);
                await _getInTouchService.CreateAsync(entity);
                TempData["SuccessMessage"] = "İletişim bilgisi başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"İletişim bilgisi eklenirken bir hata oluştu: {ex.Message}";
                return View(createGetInTouchDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["ErrorMessage"] = "Geçersiz kayıt ID. Silme işlemi gerçekleştirilemedi.";
                    return RedirectToAction("Index");
                }

                await _getInTouchService.DeleteAsync(id);
                TempData["SuccessMessage"] = "İletişim bilgisi başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"İletişim bilgisi silinirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var entity = await _getInTouchService.GetByIdAsync(id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek kayıt bulunamadı. Silinmiş veya geçersiz bir kayıt olabilir.";
                    return RedirectToAction("Index");
                }

                var update = _mapper.Map<UpdateGetInTouchDto>(entity);
                return View(update);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"İletişim bilgisi yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateGetInTouchDto updateGetInTouchDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları kontrol edin.";
                return View(updateGetInTouchDto);
            }

            try
            {
                var entity = await _getInTouchService.GetByIdAsync(updateGetInTouchDto.GetInTouchId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek kayıt bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                if (updateGetInTouchDto.ImageFile != null && updateGetInTouchDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updateGetInTouchDto.ImageFile, "getintouch");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(updateGetInTouchDto);
                    }
                    updateGetInTouchDto.ImageURL = imageUrl;
                }
                else
                {
                    updateGetInTouchDto.ImageURL = entity.ImageURL;
                }

                _mapper.Map(updateGetInTouchDto, entity);
                await _getInTouchService.UpdateAsync(entity, entity.GetInTouchId);
                TempData["SuccessMessage"] = "İletişim bilgisi başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"İletişim bilgisi güncellenirken bir hata oluştu: {ex.Message}";
                return View(updateGetInTouchDto);
            }
        }
    }
}
