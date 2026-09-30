using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.Offer;

namespace MongoDBAdmin.Controllers
{
    [Authorize(Roles = "Admin,Moderatör")]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public OfferController(IOfferService offerService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _offerService = offerService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var values = await _offerService.GetListAsync(size, page);
                return View(values);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Teklif listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOfferDto createOfferDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları doldurun.";
                return View(createOfferDto);
            }

            try
            {
                if (createOfferDto.ImageFile != null && createOfferDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(createOfferDto.ImageFile, "offers");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(createOfferDto);
                    }
                    createOfferDto.ImageURL = imageUrl;
                }

                var offer = _mapper.Map<Offer>(createOfferDto);
                offer.IsStatus = true;
                await _offerService.CreateAsync(offer);
                TempData["SuccessMessage"] = "Teklif başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Teklif eklenirken bir hata oluştu: {ex.Message}";
                return View(createOfferDto);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData["ErrorMessage"] = "Geçersiz teklif ID. Silme işlemi gerçekleştirilemedi.";
                    return RedirectToAction("Index");
                }

                await _offerService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Teklif başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Teklif silinirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(string id)
        {
            try
            {
                var value = await _offerService.GetByIdAsync(id);
                if (value == null)
                {
                    TempData["ErrorMessage"] = "Durum değiştirilecek teklif bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                value.IsStatus = !value.IsStatus;
                await _offerService.UpdateAsync(value, id);
                TempData["SuccessMessage"] = $"Teklif durumu başarıyla {(value.IsStatus ? "aktif" : "pasif")} yapıldı.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Teklif durumu değiştirilirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(string Id)
        {
            try
            {
                var offer = await _offerService.GetByIdAsync(Id);
                if (offer == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek teklif bulunamadı. Silinmiş veya geçersiz bir kayıt olabilir.";
                    return RedirectToAction("Index");
                }

                var dto = _mapper.Map<UpdateOfferDto>(offer);
                return View(dto);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Teklif bilgileri yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateOfferDto updateOfferDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları kontrol edin.";
                return View(updateOfferDto);
            }

            try
            {
                var offer = await _offerService.GetByIdAsync(updateOfferDto.OfferId);
                if (offer == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek teklif bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                if (updateOfferDto.ImageFile != null && updateOfferDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updateOfferDto.ImageFile, "offers");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin.";
                        return View(updateOfferDto);
                    }
                    updateOfferDto.ImageURL = imageUrl;
                }
                else
                {
                    updateOfferDto.ImageURL = offer.ImageURL;
                }

                _mapper.Map(updateOfferDto, offer);
                await _offerService.UpdateAsync(offer, offer.OfferId);
                TempData["SuccessMessage"] = "Teklif başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Teklif güncellenirken bir hata oluştu: {ex.Message}";
                return View(updateOfferDto);
            }
        }

    }
}
