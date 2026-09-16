using System.Collections.Generic;

namespace Application.Features.GetInTouch.Queries.GetList
{
    public class GetListGetInTouchDto
    {
        public string GetInTouchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public List<GetListGetInTouchArticleDto> GetInTouchArticles { get; set; } = new();
    }

    public class GetListGetInTouchArticleDto
    {
        public int GetInTouchArticleId { get; set; }
        public string IconURL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
