using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.Brand
{
    public class CreateBrandDto
    {
        public string BrandName { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
