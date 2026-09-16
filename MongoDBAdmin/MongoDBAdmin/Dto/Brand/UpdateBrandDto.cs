using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.Brand
{
    public class UpdateBrandDto
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string? ImageURL { get; set; }
        public bool IsStatus { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
