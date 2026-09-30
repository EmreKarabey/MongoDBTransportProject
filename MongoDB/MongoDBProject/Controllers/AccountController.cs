using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Application.Features.Login.Command.GoogleLogin;
using Application.Services.Cloudinary;
using CloudinaryDotNet.Actions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ICloudinaryService cloudinaryService,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _cloudinaryService = cloudinaryService;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl)
        {
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
                return RedirectToAction("Index", "Home");
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


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> GoogleLogin(GoogleLoginCommand googleLoginCommand)
        {
            GoogleLoginResult loginResponse = await Mediator.Send(googleLoginCommand);
            if (loginResponse.Success) return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, loginResponse.Message ?? "Google ile giriş yaparken bir hata oluştu.");
            return View("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Register(GoogleLoginCommand googleLoginCommand)
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
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
                UserName = registerDto.UserName,
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
            catch (Exception ex)
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

            var result2 = await _userManager.AddToRoleAsync(user, "Üye");
            if (!result2.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                TempData["ErrorMessage"] = $"Rol atanırken bir hata oluştu: {errors}";
                return RedirectToAction("UserList");
            }

            if (User.Identity?.IsAuthenticated == true)
            {
                TempData["SuccessMessage"] = $"'{user.UserName}' kullanıcı adına sahip yeni hesap başarıyla oluşturuldu.";
                return RedirectToAction("Login");
            }
            return View(registerDto);
        }



        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Lütfen geçerli bir e-posta adresi giriniz.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Girdiğiniz e-posta adresiyle eşleşen bir hesap bulunamadı.");
                return View();
            }

            // 6 haneli doğrulama kodu üret ve 2 dakika süre ver
            int code = RandomNumberGenerator.GetInt32(100000, 1000000);
            var codeExpiryTime = DateTime.UtcNow.AddMinutes(2);

            user.Code = code;
            user.CodeExpiryTime = codeExpiryTime;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Doğrulama kodu oluşturulurken bir hata meydana geldi. Lütfen tekrar deneyiniz.");
                return View();
            }

            string subject = "Şifre Sıfırlama Doğrulama Kodunuz";
            string recipientName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : (user.UserName ?? "Kullanıcı");

            string htmlMessage = $@"
            <div style=""font-family: 'Segoe UI', Arial, sans-serif; max-width: 580px; margin: 0 auto; background: #ffffff; border-radius: 12px; overflow: hidden; border: 1px solid #e9ecef; box-shadow: 0 4px 15px rgba(0,0,0,0.05);"">
                <div style=""background: #0E0E0E; padding: 25px 30px; text-align: center;"">
                    <h2 style=""color: #ffffff; margin: 0; font-size: 20px; font-weight: 700; letter-spacing: 0.5px;"">Hesap Doğrulama</h2>
                </div>
                <div style=""padding: 35px 30px;"">
                    <p style=""font-size: 15px; color: #2B3445; line-height: 1.6; margin-top: 0;"">
                        Merhaba <strong>{recipientName}</strong>,
                    </p>
                    <p style=""font-size: 14px; color: #606F7B; line-height: 1.6;"">
                        Hesabınızın şifresini sıfırlamak için bir talepte bulundunuz. Şifrenizi yenilemek için aşağıdaki 6 haneli tek kullanımlık doğrulama kodunu kullanabilirsiniz:
                    </p>
                    <div style=""text-align: center; margin: 30px 0; background: #F8F9FA; padding: 20px; border-radius: 10px; border: 2px dashed #FF5D47;"">
                        <span style=""font-size: 34px; font-weight: 800; letter-spacing: 8px; color: #FF5D47; font-family: monospace; display: inline-block;"">{code}</span>
                    </div>
                    <div style=""background-color: #FFF8E6; border-left: 4px solid #F59E0B; padding: 12px 16px; border-radius: 6px; margin-bottom: 25px;"">
                        <p style=""margin: 0; font-size: 13px; color: #8F5B00; font-weight: 600;"">
                            ⏱ Bu kod güvenlik nedeniyle <strong>2 dakika</strong> boyunca geçerlidir. Süre dolduktan sonra geçerliliğini yitirecektir.
                        </p>
                    </div>
                    <p style=""font-size: 12px; color: #8795A1; line-height: 1.6; border-top: 1px solid #EEF2F5; padding-top: 20px; margin-bottom: 0;"">
                        Bu talebi siz yapmadıysanız lütfen bu e-postayı görmezden geliniz. Hesabınız güvendedir.
                    </p>
                </div>
                <div style=""background: #F8F9FA; padding: 15px; text-align: center; font-size: 12px; color: #A0AEC0; border-top: 1px solid #EDF2F7;"">
                    &copy; {DateTime.UtcNow.Year} Taşıma & Lojistik Sistemi. Tüm hakları saklıdır.
                </div>
            </div>";

            try
            {
                await _emailSender.SendEmailAsync(user.Email!, subject, htmlMessage);
                TempData["SuccessMessage"] = "Şifre sıfırlama kodunuz e-posta adresinize gönderildi. Lütfen gelen kutunuzdaki kodu girerek yeni şifrenizi belirleyiniz.";
                return RedirectToAction("ResetPassword", new { email = user.Email });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "E-posta gönderilirken bir hata oluştu: " + ex.Message);
                return View();
            }
        }


        [HttpGet]
        public IActionResult ResetPassword(string? email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string email, int code, string newPassword, string confirmPassword)
        {
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "E-posta adresi boş bırakılamaz.");
                return View();
            }

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError(string.Empty, "Lütfen yeni şifrenizi giriniz.");
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Girdiğiniz yeni şifreler birbiriyle uyuşmuyor.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Bu e-posta adresine ait bir hesap bulunamadı.");
                return View();
            }

            if (user.Code != code || user.CodeExpiryTime == null || user.CodeExpiryTime < DateTime.UtcNow)
            {
                ModelState.AddModelError(string.Empty, "Girdiğiniz doğrulama kodu hatalı veya 2 dakikalık süresi dolmuş.");
                return View();
            }

            // Şifre sıfırlama tokeni oluştur ve şifreyi güncelle
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }


            user.Code = null;
            user.CodeExpiryTime = null;
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "Şifreniz başarıyla güncellendi! Yeni şifrenizle giriş yapabilirsiniz.";
            return RedirectToAction("Login");
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
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


    public record LoginDto
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
