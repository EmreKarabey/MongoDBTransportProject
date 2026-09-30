using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDBAdmin.Dto.Brand;

namespace MongoDBAdmin.Controllers
{
    [Authorize(Roles = "Admin,Moderatör")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppUserService _appUserService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ICloudinaryService _cloudinaryService;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAppUserService appUserService, RoleManager<AppRole> roleManager, ICloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appUserService = appUserService;
            _roleManager = roleManager;
            _cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var user = await _userManager.FindByNameAsync(loginDto.UserName)
                       ?? await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Girdiğiniz kullanıcı adı veya e-posta adresiyle eşleşen bir hesap bulunamadı.");
                return View(loginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, loginDto.Password, loginDto.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Brand");
            }

            if (result.IsLockedOut)
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                var remainingMinutes = lockoutEnd.HasValue
                    ? Math.Max(1, (int)Math.Ceiling((lockoutEnd.Value - DateTimeOffset.UtcNow).TotalMinutes))
                    : 5;

                ModelState.AddModelError(string.Empty, $"Çok fazla hatalı giriş denemesi yaptığınız için hesabınız geçici olarak kilitlenmiştir. Lütfen yaklaşık {remainingMinutes} dakika sonra tekrar deneyiniz.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Bu hesabın sisteme giriş izni bulunmuyor veya e-posta adresi henüz onaylanmamış.");
            }
            else if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError(string.Empty, "Bu hesap için iki aşamalı doğrulama (2FA) gerekmektedir.");
            }
            else
            {
                var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                var remaining = maxAttempts - failedCount;

                if (remaining > 0)
                {
                    ModelState.AddModelError(string.Empty, $"Girdiğiniz şifre hatalıdır. Lütfen kontrol edip tekrar deneyiniz. (Kalan deneme hakkı: {remaining})");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Şifrenizi art arda hatalı girdiğiniz için hesabınız geçici olarak kilitlenmiştir.");
                }
            }

            return View(loginDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var existingUserByUserName = await _userManager.FindByNameAsync(registerDto.UserName);
            if (existingUserByUserName != null)
            {
                ModelState.AddModelError(nameof(registerDto.UserName), "Bu kullanıcı adı zaten başka bir üye tarafından kullanılmaktadır.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUserByEmail != null)
            {
                ModelState.AddModelError(nameof(registerDto.Email), "Bu e-posta adresi ile kayıtlı bir hesap zaten mevcuttur.");
            }

            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.UserName
            };

            try
            {
                if (registerDto.ImageFile != null && registerDto.ImageFile.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(registerDto.ImageFile, "users");
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        TempData["ErrorMessage"] = "Görsel Cloudinary'ye yüklenemedi. Lütfen geçerli bir görsel dosyası seçin ve tekrar deneyin.";
                        return View(registerDto);
                    }
                    user.ImageURL = imageUrl;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    string turkishMessage = item.Code switch
                    {
                        "DuplicateUserName" => "Bu kullanıcı adı sistemde zaten kayıtlı.",
                        "DuplicateEmail" => "Bu e-posta adresi sistemde zaten kayıtlı.",
                        "PasswordTooShort" => "Şifre en az 6 karakter uzunluğunda olmalıdır.",
                        "PasswordRequiresNonAlphanumeric" => "Şifreniz en az bir özel karakter (@, #, $, !, . vb.) içermelidir.",
                        "PasswordRequiresDigit" => "Şifreniz en az bir rakam ('0'-'9') içermelidir.",
                        "PasswordRequiresLower" => "Şifreniz en az bir küçük harf ('a'-'z') içermelidir.",
                        "PasswordRequiresUpper" => "Şifreniz en az bir büyük harf ('A'-'Z') içermelidir.",
                        "InvalidUserName" => "Kullanıcı adı yalnızca harf, rakam ve izin verilen karakterleri içerebilir.",
                        "InvalidEmail" => "Lütfen geçerli bir e-posta formatı giriniz.",
                        _ => item.Description
                    };

                    if (item.Code.Contains("Password", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(registerDto.Password), turkishMessage);
                    }
                    else if (item.Code.Contains("UserName", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(registerDto.UserName), turkishMessage);
                    }
                    else if (item.Code.Contains("Email", StringComparison.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(nameof(registerDto.Email), turkishMessage);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, turkishMessage);
                    }
                }
                return View(registerDto);
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                TempData["SuccessMessage"] = $"'{user.UserName}' kullanıcı adına sahip yeni hesap başarıyla oluşturuldu.";
                return RedirectToAction("Register");
            }
            return RedirectToAction("UserList");
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserList()
        {
            var list = await _appUserService.GetUserListAsync();
            return View(list);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AssigneRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                TempData["ErrorMessage"] = "Kullanıcı veya rol bilgisi eksik. Lütfen tekrar deneyin.";
                return RedirectToAction("UserList");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "İşlem yapılmak istenen kullanıcı bulunamadı.";
                    return RedirectToAction("UserList");
                }

                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    TempData["ErrorMessage"] = "Atanmak istenen rol bulunamadı.";
                    return RedirectToAction("UserList");
                }


                var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
                if (isInRole)
                {
                    TempData["ErrorMessage"] = $"Kullanıcı zaten '{role.Name}' rolüne sahip.";
                    return RedirectToAction("UserList");
                }

                var result = await _userManager.AddToRoleAsync(user, role.Name);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    TempData["ErrorMessage"] = $"Rol atanırken bir hata oluştu: {errors}";
                    return RedirectToAction("UserList");
                }


                await _userManager.UpdateAsync(user);

                TempData["SuccessMessage"] = $"Kullanıcıya '{role.Name}' rolü başarıyla atandı.";
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Beklenmedik bir hata oluştu: {ex.Message}";
                return RedirectToAction("UserList");
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Hesabınızdan güvenli bir şekilde çıkış yapıldı.";
            return RedirectToAction("Login", "Account");
        }
    }

    public class RegisterDto
    {
        [Required(ErrorMessage = "Ad ve Soyad alanı zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kullanıcı adı alanı zorunludur.")]
        [MinLength(3, ErrorMessage = "Kullanıcı adı en az 3 karakter olmalıdır.")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-Posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı alanı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler birbiriyle eşleşmiyor.")]
        [Display(Name = "Şifre Tekrarı")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public IFormFile? ImageFile { get; set; }
    }

    public class LoginDto
    {
        [Required(ErrorMessage = "Kullanıcı adı veya e-posta alanı zorunludur.")]
        [Display(Name = "Kullanıcı Adı veya E-Posta")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
