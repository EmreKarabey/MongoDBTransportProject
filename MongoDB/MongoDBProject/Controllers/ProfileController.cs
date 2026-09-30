using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Services.Cloudinary;
using Application.Services.Repository;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDBProject.Models;

namespace MongoDBProject.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ProfileController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ICloudinaryService cloudinaryService,
            IShipmentRepository shipmentRepository,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cloudinaryService = cloudinaryService;
            _shipmentRepository = shipmentRepository;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);
            int shipmentCount = 0;
            try
            {
                var shipments = await _shipmentRepository.OnlyListAsync(s => s.SenderUserId == user.Id || s.ReceiverUserId == user.Id);
                shipmentCount = shipments?.Count ?? 0;
            }
            catch
            {

                shipmentCount = 0;
            }

            var model = _mapper.Map<ProfileViewModel>(user);
            model.Roles = roles;
            model.TotalShipments = shipmentCount;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var roles = await _userManager.GetRolesAsync(user);
            model.Roles = roles;
            model.ExistingImageUrl = user.ImageURL;
            try
            {
                var shipments = await _shipmentRepository.OnlyListAsync(s => s.SenderUserId == user.Id || s.ReceiverUserId == user.Id);
                model.TotalShipments = shipments?.Count ?? 0;
            }
            catch
            {
                model.TotalShipments = 0;
            }

            if (!string.Equals(user.UserName, model.UserName, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userManager.FindByNameAsync(model.UserName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı başka bir kullanıcı tarafından kullanılmaktadır.");
                }
            }

            if (!string.Equals(user.Email, model.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmail != null && existingEmail.Id != user.Id)
                {
                    ModelState.AddModelError(nameof(model.Email), "Bu e-posta adresi başka bir kullanıcı tarafından kullanılmaktadır.");
                }
            }

            bool isChangingPassword = !string.IsNullOrWhiteSpace(model.NewPassword);
            if (isChangingPassword)
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "Şifrenizi güncellemek için mevcut şifrenizi girmelisiniz.");
                }

                if (model.NewPassword.Length < 6)
                {
                    ModelState.AddModelError(nameof(model.NewPassword), "Yeni şifre en az 6 karakter olmalıdır.");
                }

                if (model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmNewPassword), "Yeni şifreler birbiriyle uyuşmuyor.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.RemoveImage)
            {
                user.ImageURL = null;
                model.ExistingImageUrl = null;
            }
            else if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string? uploadedUrl = null;

                try
                {
                    uploadedUrl = await _cloudinaryService.UploadImageAsync(model.ImageFile, "users");
                }
                catch
                {
                    uploadedUrl = null;
                }

                if (string.IsNullOrEmpty(uploadedUrl))
                {
                    try
                    {
                        var webRoot = _webHostEnvironment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                        var uploadsFolder = Path.Combine(webRoot, "uploads", "profiles");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var fileExtension = Path.GetExtension(model.ImageFile.FileName);
                        if (string.IsNullOrEmpty(fileExtension)) fileExtension = ".jpg";
                        var uniqueFileName = $"{Guid.NewGuid():N}{fileExtension}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(fileStream);
                        }

                        uploadedUrl = $"/uploads/profiles/{uniqueFileName}";
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Profil resmi kaydedilirken bir hata oluştu: " + ex.Message);
                        return View(model);
                    }
                }

                if (!string.IsNullOrEmpty(uploadedUrl))
                {
                    user.ImageURL = uploadedUrl;
                    model.ExistingImageUrl = uploadedUrl;
                }
            }

            _mapper.Map(model, user);

            if (isChangingPassword && !string.IsNullOrWhiteSpace(model.CurrentPassword) && !string.IsNullOrWhiteSpace(model.NewPassword))
            {
                var passResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passResult.Succeeded)
                {
                    foreach (var error in passResult.Errors)
                    {
                        string message = error.Code switch
                        {
                            "PasswordMismatch" => "Mevcut şifreniz hatalı. Lütfen kontrol edip tekrar deneyiniz.",
                            "PasswordTooShort" => "Yeni şifreniz en az 6 karakter olmalıdır.",
                            "PasswordRequiresDigit" => "Yeni şifreniz en az bir rakam içermelidir.",
                            "PasswordRequiresUpper" => "Yeni şifreniz en az bir büyük harf içermelidir.",
                            "PasswordRequiresLower" => "Yeni şifreniz en az bir küçük harf içermelidir.",
                            "PasswordRequiresNonAlphanumeric" => "Yeni şifreniz en az bir özel karakter içermelidir.",
                            _ => error.Description
                        };
                        ModelState.AddModelError(nameof(model.CurrentPassword), message);
                    }
                    return View(model);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Index");
        }
    }
}
