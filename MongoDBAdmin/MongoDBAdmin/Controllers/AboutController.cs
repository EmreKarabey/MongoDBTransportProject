using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.About;

namespace MongoDBAdmin.Controllers
{
    [AutoValidateAntiforgeryToken]
    [Authorize(Roles = "Admin,Moderatör")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public AboutController(IAboutService aboutService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _aboutService = aboutService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _aboutService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hakkımızda listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddAbout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddAbout(CreateAboutDto createAboutDto)
        {
            if (createAboutDto.IsActive)
            {
                var any = await _aboutService.AnyActive();

                if (any)
                {
                    TempData["Error"] = "Zaten aktif bir Hakkımızda kaydı mevcut! Yeni bir aktif kayıt eklemek için önce mevcut aktif kaydı pasife alın.";
                    return View(createAboutDto);
                }
            }

            try
            {
                if (createAboutDto.ImageFile != null && createAboutDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(createAboutDto.ImageFile, "abouts");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(createAboutDto);
                    }
                    createAboutDto.ImageURL = imageUrl;
                }

                var about = _mapper.Map<About>(createAboutDto);
                await _aboutService.CreateAsync(about);
                TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Hakkımızda bilgisi eklenirken bir hata oluştu: {ex.Message}";
                return View(createAboutDto);
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

                await _aboutService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Silme işlemi sırasında bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeIsActive(string id)
        {
            try
            {
                var entity = await _aboutService.GetByIdAsync(id);

                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Hakkımızda kaydı bulunamadı! Kayıt silinmiş veya geçersiz bir işlem yapılmış olabilir.";
                    return RedirectToAction("Index");
                }

                entity.IsActive = !entity.IsActive;

                if (entity.IsActive)
                {
                    var any = await _aboutService.AnyActive();

                    if (any)
                    {
                        TempData["ErrorMessage"] = "Zaten aktif bir Hakkımızda kaydı mevcut! Yeni bir aktif kayıt eklemek için önce mevcut aktif kaydı pasife alın.";
                        return RedirectToAction("Index");
                    }
                }

                await _aboutService.UpdateAsync(entity, entity.AboutId);
                TempData["SuccessMessage"] = "Hakkımızda durumu başarıyla değiştirildi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Durum değiştirme sırasında bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var entity = await _aboutService.GetByIdAsync(id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek kayıt bulunamadı. Silinmiş veya geçersiz bir kayıt olabilir.";
                    return RedirectToAction("Index");
                }

                var update = _mapper.Map<UpdateAboutDto>(entity);
                return View(update);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bilgi yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAboutDto updateAboutDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları kontrol edin.";
                return View(updateAboutDto);
            }

            try
            {
                var entity = await _aboutService.GetByIdAsync(updateAboutDto.AboutId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek kayıt bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                if (updateAboutDto.ImageFile != null && updateAboutDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updateAboutDto.ImageFile, "abouts");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(updateAboutDto);
                    }
                    updateAboutDto.ImageURL = imageUrl;
                }
                else
                {
                    updateAboutDto.ImageURL = entity.ImageURL;
                }

                _mapper.Map(updateAboutDto, entity);

                if (entity.IsActive)
                {
                    var any = await _aboutService.AnyActive();

                    if (any)
                    {
                        TempData["ErrorMessage"] = "Zaten aktif bir Hakkımızda kaydı mevcut! Güncelleme iptal edildi. Önce mevcut aktif kaydı pasife alın.";
                        return View(updateAboutDto);
                    }
                }

                await _aboutService.UpdateAsync(entity, entity.AboutId);
                TempData["SuccessMessage"] = "Hakkımızda bilgisi başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Güncelleme sırasında bir hata oluştu: {ex.Message}";
                return View(updateAboutDto);
            }
        }
    }
}
