using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MongoDBProject.Models
{
    public class ProfileViewModel
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

        [Display(Name = "Telefon Numarası")]
        [Phone(ErrorMessage = "Lütfen geçerli bir telefon numarası giriniz.")]
        public string? PhoneNumber { get; set; }

        public string? ExistingImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool RemoveImage { get; set; } = false;

        public IList<string> Roles { get; set; } = new List<string>();

        public int TotalShipments { get; set; } = 0;

        // Şifre Değiştirme (İsteğe bağlı)
        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string? CurrentPassword { get; set; }

        [MinLength(6, ErrorMessage = "Yeni şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Yeni şifreler birbiriyle uyuşmuyor.")]
        [Display(Name = "Yeni Şifre Tekrar")]
        public string? ConfirmNewPassword { get; set; }
    }
}
