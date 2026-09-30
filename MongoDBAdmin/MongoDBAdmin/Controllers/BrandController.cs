using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.Brand;

namespace MongoDBAdmin.Controllers
{
    [Authorize(Roles = "Admin,Moderatör")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;

        public BrandController(IBrandService brandService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _brandService = brandService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index(int size = 9, int page = 1)
        {
            try
            {
                var list = await _brandService.GetListAsync(size, page);
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka listesi yüklenirken bir hata oluştu: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateBrand()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand(CreateBrandDto createBrandDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları doldurun.";
                return View(createBrandDto);
            }

            try
            {
                if (createBrandDto.ImageFile != null && createBrandDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(createBrandDto.ImageFile, "brands");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel dosyası seçin ve tekrar deneyin.";
                        return View(createBrandDto);
                    }
                    createBrandDto.ImageURL = imageUrl;
                }

                var entity = _mapper.Map<Brand>(createBrandDto);
                entity.IsStatus = true;

                await _brandService.CreateAsync(entity);
                TempData["SuccessMessage"] = $"'{createBrandDto.BrandName}' markası başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka eklenirken bir hata oluştu: {ex.Message}";
                return View(createBrandDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateBrand(string Id)
        {
            try
            {
                var entity = await _brandService.GetByIdAsync(Id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek marka bulunamadı. Silinmiş veya geçersiz bir kayıt olabilir.";
                    return RedirectToAction("Index");
                }

                var mapper = _mapper.Map<UpdateBrandDto>(entity);
                return View(mapper);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka bilgileri yüklenirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBrand(UpdateBrandDto updateBrandDto)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Form alanları geçersiz. Lütfen tüm zorunlu alanları kontrol edin.";
                return View(updateBrandDto);
            }

            try
            {
                var entity = await _brandService.GetByIdAsync(updateBrandDto.BrandId);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Güncellenecek marka bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                if (updateBrandDto.ImageFile != null && updateBrandDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(updateBrandDto.ImageFile, "brands");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Yeni görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel seçin ve tekrar deneyin.";
                        return View(updateBrandDto);
                    }
                    updateBrandDto.ImageURL = imageUrl;
                }
                else
                {
                    updateBrandDto.ImageURL = entity.ImageURL;
                }

                _mapper.Map(updateBrandDto, entity);
                await _brandService.UpdateAsync(entity, entity.BrandId);
                TempData["SuccessMessage"] = $"'{entity.BrandName}' markası başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka güncellenirken bir hata oluştu: {ex.Message}";
                return View(updateBrandDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeActive(string Id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(Id);
                if (brand == null)
                {
                    TempData["ErrorMessage"] = "Aktif yapılacak marka bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                brand.IsStatus = true;
                await _brandService.UpdateAsync(brand, brand.BrandId);
                TempData["SuccessMessage"] = $"'{brand.BrandName}' markası aktif duruma getirildi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka aktif yapılırken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassive(string Id)
        {
            try
            {
                var brand = await _brandService.GetByIdAsync(Id);
                if (brand == null)
                {
                    TempData["ErrorMessage"] = "Pasif yapılacak marka bulunamadı. Kayıt silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                brand.IsStatus = false;
                await _brandService.UpdateAsync(brand, brand.BrandId);
                TempData["SuccessMessage"] = $"'{brand.BrandName}' markası pasif duruma alındı.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka pasif yapılırken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBrand(string Id)
        {
            try
            {
                var entity = await _brandService.GetByIdAsync(Id);
                if (entity == null)
                {
                    TempData["ErrorMessage"] = "Silinecek marka bulunamadı. Kayıt zaten silinmiş olabilir.";
                    return RedirectToAction("Index");
                }

                await _brandService.DeleteAsync(entity.BrandId);
                TempData["SuccessMessage"] = $"'{entity.BrandName}' markası başarıyla silindi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Marka silinirken bir hata oluştu: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
