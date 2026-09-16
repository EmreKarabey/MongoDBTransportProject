using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.Slider
{
    public class CreateSliderDto
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
