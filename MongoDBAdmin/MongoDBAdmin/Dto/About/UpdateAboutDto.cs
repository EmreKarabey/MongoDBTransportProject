using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.About
{
    public class UpdateAboutDto
    {
        public string AboutId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? Article { get; set; }
        public string? Article2 { get; set; }
        public string? Article3 { get; set; }
        public string? Article4 { get; set; }
        public string? Article5 { get; set; }
        public string? Article6 { get; set; }
        public bool IsActive { get; set; }
    }
}
