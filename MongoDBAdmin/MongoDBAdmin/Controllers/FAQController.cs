using System;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.FAQ;

namespace MongoDBAdmin.Controllers
{
    [Authorize]
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;
        private readonly IMapper _mapper;

        public FAQController(IFAQService faqService, IMapper mapper)
        {
            _faqService = faqService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _faqService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Liste yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateFAQ()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFAQ(CreateFAQDto createDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu alanları doldurun.";
                return View(createDto);
            }

            try
            {
                var entity = _mapper.Map<FAQ>(createDto);
                entity.IsActive = true;

                await _faqService.CreateAsync(entity);
                TempData["SuccessMessage"] = "FAQ başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Eklenirken hata oluştu: {ex.Message}";
                return View(createDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateFAQ(string id)
        {
            try
            {
                var entity = await _faqService.GetByIdAsync(id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Kayıt bulunamadı.";
                    return RedirectToAction("Index");
                }

                var updateDto = _mapper.Map<UpdateFAQDto>(entity);
                return View(updateDto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Yüklenirken hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateFAQ(UpdateFAQDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu alanları kontrol edin.";
                return View(updateDto);
            }

            try
            {
                var entity = await _faqService.GetByIdAsync(updateDto.FaqId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Kayıt bulunamadı.";
                    return RedirectToAction("Index");
                }

                _mapper.Map(updateDto, entity);
                await _faqService.UpdateAsync(entity, entity.FaqId);

                TempData["SuccessMessage"] = "FAQ başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Güncellenirken hata oluştu: {ex.Message}";
                return View(updateDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string id)
        {
            try
            {
                var entity = await _faqService.GetByIdAsync(id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Kayıt bulunamadı." });
                }

                entity.IsActive = !entity.IsActive;
                await _faqService.UpdateAsync(entity, entity.FaqId);

                return Json(new { success = true, isActive = entity.IsActive, message = "Durum başarıyla değiştirildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFAQ(string id)
        {
            try
            {
                var entity = await _faqService.GetByIdAsync(id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Kayıt bulunamadı." });
                }

                await _faqService.DeleteAsync(entity.FaqId);
                return Json(new { success = true, message = "Başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
