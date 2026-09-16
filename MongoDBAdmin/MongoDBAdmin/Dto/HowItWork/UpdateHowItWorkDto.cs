using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.HowItWork
{
    public class UpdateHowItWorkDto
    {
        public string HowItWorkId { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<UpdateHowItWorkArticleDto> HowItWorkArticles { get; set; } = new();
    }

    public class UpdateHowItWorkArticleDto
    {
        public int HowItWorkArticleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
