using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.HowItWork;

namespace MongoDBAdmin.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class HowItWorkController : Controller
    {
        private readonly IHowItWorkService _howItWorkService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public HowItWorkController(IHowItWorkService howItWorkService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _howItWorkService = howItWorkService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var list = await _howItWorkService.GetListAsync(10, 1);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Nasıl çalışır listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult AddHowItWork()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHowItWork(CreateHowItWorkDto createHowItWorkDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları doldurun.";
                return View(createHowItWorkDto);
            }

            try
            {
                if (createHowItWorkDto.ImageFile != null && createHowItWorkDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(createHowItWorkDto.ImageFile, "howitworks");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(createHowItWorkDto);
                    }
                    createHowItWorkDto.ImageURL = imageUrl;
                }

                var entity = _mapper.Map<HowItWork>(createHowItWorkDto);
                await _howItWorkService.CreateAsync(entity);
                TempData["SuccessMessage"] = "Nasıl çalışır bilgisi başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bilgi eklenirken bir hata oluştu: {ex.Message}";
                return View(createHowItWorkDto);
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

                await _howItWorkService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Nasıl çalışır bilgisi başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Silme işlemi sırasında bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            try
            {
                var entity = await _howItWorkService.GetByIdAsync(id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek kayıt bulunamadı. Silinmiş veya geçersiz bir kayıt olabilir.";
                    return RedirectToAction("Index");
                }

                var update = _mapper.Map<UpdateHowItWorkDto>(entity);
                return View(update);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Bilgi yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateHowItWorkDto updateHowItWorkDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları kontrol edin.";
                return View(updateHowItWorkDto);
            }

            try
            {
                var entity = await _howItWorkService.GetByIdAsync(updateHowItWorkDto.HowItWorkId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek kayıt bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                if (updateHowItWorkDto.ImageFile != null && updateHowItWorkDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updateHowItWorkDto.ImageFile, "howitworks");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(updateHowItWorkDto);
                    }
                    updateHowItWorkDto.ImageURL = imageUrl;
                }
                else
                {
                    updateHowItWorkDto.ImageURL = entity.ImageURL;
                }

                _mapper.Map(updateHowItWorkDto, entity);
                await _howItWorkService.UpdateAsync(entity, entity.HowItWorkId);
                TempData["SuccessMessage"] = "Nasıl çalışır bilgisi başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Güncelleme sırasında bir hata oluştu: {ex.Message}";
                return View(updateHowItWorkDto);
            }
        }
    }
}
