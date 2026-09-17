using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.Slider;
using MongoDBAdmin.Models;

namespace MongoDBAdmin.Controllers
{
    [Authorize]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly ILogger<SliderController> _logger;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public SliderController(ILogger<SliderController> logger, ISliderService sliderService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _logger = logger;
            _sliderService = sliderService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _sliderService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Slider listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(CreateSliderDto slider)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları doldurun.";
                return View(slider);
            }

            try
            {
                if (slider.ImageFile != null && slider.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(slider.ImageFile, "sliders");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin ve tekrar deneyin.";
                        return View(slider);
                    }
                    slider.ImageURL = imageUrl;
                }

                var entity = _mapper.Map<Slider>(slider);
                await _sliderService.CreateAsync(entity);
                TempData["SuccessMessage"] = "Slider başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Slider eklenirken bir hata oluştu: {ex.Message}";
                return View(slider);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSlider(string Id)
        {
            try
            {
                var entity = await _sliderService.GetByIdAsync(Id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek slider bulunamadı. Silinmiş veya geçersiz bir kayıt olabilir.";
                    return RedirectToAction("Index");
                }

                var mapper = _mapper.Map<UpdateSliderDto>(entity);
                return View(mapper);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Slider bilgileri yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSlider(UpdateSliderDto slider)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları kontrol edin.";
                return View(slider);
            }

            try
            {
                var entity = await _sliderService.GetByIdAsync(slider.SliderId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek slider bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                if (slider.ImageFile != null && slider.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(slider.ImageFile, "sliders");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin ve tekrar deneyin.";
                        return View(slider);
                    }
                    slider.ImageURL = imageUrl;
                }
                else
                {
                    slider.ImageURL = entity.ImageURL;
                }

                _mapper.Map(slider, entity);
                await _sliderService.UpdateAsync(entity, entity.SliderId);
                TempData["SuccessMessage"] = "Slider başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Slider güncellenirken bir hata oluştu: {ex.Message}";
                return View(slider);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSlider(string Id)
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    TempData["ErrorMessage"] = "Geçersiz slider ID. Silme işlemi gerçekleştirilemedi.";
                    return RedirectToAction("Index");
                }

                await _sliderService.DeleteAsync(Id);
                TempData["SuccessMessage"] = "Slider başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Slider silinirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
