using EntityLayer.Entities;
using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.GetInTouch
{
    public class CreateGetInTouchDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<CreateGetInTouchArticleDto> GetInTouchArticles { get; set; } = new();
    }

    public class CreateGetInTouchArticleDto
    {
        public int GetInTouchArticleId { get; set; }
        public string IconURL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
