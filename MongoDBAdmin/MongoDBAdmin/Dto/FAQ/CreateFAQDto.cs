using System.ComponentModel.DataAnnotations;

namespace MongoDBAdmin.Dto.FAQ
{
    public class CreateFAQDto
    {
        [Required(ErrorMessage = "Soru alanı zorunludur.")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
