using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Features.Login.Command.GoogleLogin;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBProject.Controllers
{
    public class AccountController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto,string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var user = await _userManager.FindByNameAsync(loginDto.UserName)
                      ?? await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Girdiðiniz kullanýcý adý veya e-posta adresiyle eþleþen bir hesap bulunamadý.");
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

                ModelState.AddModelError(string.Empty, $"Çok fazla hatalý giriþ denemesi yaptýðýnýz için hesabýnýz geçici olarak kilitlenmiþtir. Lütfen yaklaþýk {remainingMinutes} dakika sonra tekrar deneyiniz.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Bu hesabýn sisteme giriþ izni bulunmuyor veya e-posta adresi henüz onaylanmamýþ.");
            }
            else if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError(string.Empty, "Bu hesap için iki aþamalý doðrulama (2FA) gerekmektedir.");
            }
            else
            {
                var failedCount = await _userManager.GetAccessFailedCountAsync(user);
                var maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                var remaining = maxAttempts - failedCount;

                if (remaining > 0)
                {
                    ModelState.AddModelError(string.Empty, $"Girdiðiniz þifre hatalýdýr. Lütfen kontrol edip tekrar deneyiniz. (Kalan deneme hakký: {remaining})");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Þifrenizi art arda hatalý girdiðiniz için hesabýnýz geçici olarak kilitlenmiþtir.");
                }
            }

            return View(loginDto);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task <IActionResult> GoogleLogin(GoogleLoginCommand googleLoginCommand)
        {
            GoogleLoginResult loginResponse = await Mediator.Send(googleLoginCommand);
            if (loginResponse.Success) return RedirectToAction("Index","Home");
            
            ModelState.AddModelError(string.Empty, loginResponse.Message ?? "Google ile giriþ yaparken bir hata oluþtu.");
            return View("Login");
        }
    }

    public record LoginDto
    {
        [Required(ErrorMessage = "Kullanýcý adý veya e-posta alaný zorunludur.")]
        [Display(Name = "Kullanýcý Adý veya E-Posta")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Þifre alaný zorunludur.")]
        [MinLength(6, ErrorMessage = "Þifre en az 6 karakter olmalýdýr.")]
        [DataType(DataType.Password)]
        [Display(Name = "Þifre")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
