using EntityLayer.Entities;
using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.GetInTouch
{
    public class UpdateGetInTouchDto
    {
        public string GetInTouchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<UpdateGetInTouchArticleDto> GetInTouchArticles { get; set; } = new();
    }

    public class UpdateGetInTouchArticleDto
    {
        public int GetInTouchArticleId { get; set; }
        public string IconURL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
