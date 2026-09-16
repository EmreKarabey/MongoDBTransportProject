using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.HowItWork
{
    public class CreateHowItWorkDto
    {
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public IFormFile? ImageFile { get; set; }
        public List<CreateHowItWorkArticleDto> HowItWorkArticles { get; set; } = new();
    }

    public class CreateHowItWorkArticleDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
