using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.WhatWeHaveDone;

namespace MongoDBAdmin.Controllers
{
    [Authorize]
    public class WhatWeHaveDoneController : Controller
    {
        private readonly IWhatWeHaveDoneService _whatWeHaveDoneService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public WhatWeHaveDoneController(IWhatWeHaveDoneService whatWeHaveDoneService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _whatWeHaveDoneService = whatWeHaveDoneService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _whatWeHaveDoneService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Liste yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateWhatWeHaveDone()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWhatWeHaveDone(CreateWhatWeHaveDoneDto createDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu alanları doldurun.";
                return View(createDto);
            }

            try
            {
                if (createDto.ImageFile != null && createDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(createDto.ImageFile, "whatwehavedone");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Resim yüklenemedi.";
                        return View(createDto);
                    }
                    createDto.ImageURL = imageUrl;
                }

                var entity = _mapper.Map<WhatWeHaveDone>(createDto);
                entity.IsActive = true; 

                await _whatWeHaveDoneService.CreateAsync(entity);
                TempData["SuccessMessage"] = "Başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Eklenirken hata oluştu: {ex.Message}";
                return View(createDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWhatWeHaveDone(string Id)
        {
            try
            {
                var entity = await _whatWeHaveDoneService.GetByIdAsync(Id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Kayıt bulunamadı.";
                    return RedirectToAction("Index");
                }

                var updateDto = _mapper.Map<UpdateWhatWeHaveDoneDto>(entity);
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
        public async Task<IActionResult> UpdateWhatWeHaveDone(UpdateWhatWeHaveDoneDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Lütfen zorunlu alanları kontrol edin.";
                return View(updateDto);
            }

            try
            {
                var entity = await _whatWeHaveDoneService.GetByIdAsync(updateDto.WhatWeHaveDoneId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Kayıt bulunamadı.";
                    return RedirectToAction("Index");
                }

                if (updateDto.ImageFile != null && updateDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updateDto.ImageFile, "whatwehavedone");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni resim yüklenemedi.";
                        return View(updateDto);
                    }
                    updateDto.ImageURL = imageUrl;
                }
                else
                {
                    updateDto.ImageURL = entity.ImageURL;
                }

                _mapper.Map(updateDto, entity);
                await _whatWeHaveDoneService.UpdateAsync(entity, entity.WhatWeHaveDoneId);
                
                TempData["SuccessMessage"] = "Başarıyla güncellendi.";
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
                var entity = await _whatWeHaveDoneService.GetByIdAsync(id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Kayıt bulunamadı." });
                }

                entity.IsActive = !entity.IsActive;
                await _whatWeHaveDoneService.UpdateAsync(entity, entity.WhatWeHaveDoneId);

                return Json(new { success = true, isActive = entity.IsActive, message = "Durum başarıyla değiştirildi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWhatWeHaveDone(string id)
        {
            try
            {
                var entity = await _whatWeHaveDoneService.GetByIdAsync(id);
                if (entity == null)
                {
                    return Json(new { success = false, message = "Kayıt bulunamadı." });
                }

                await _whatWeHaveDoneService.DeleteAsync(entity.WhatWeHaveDoneId);
                return Json(new { success = true, message = "Başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
