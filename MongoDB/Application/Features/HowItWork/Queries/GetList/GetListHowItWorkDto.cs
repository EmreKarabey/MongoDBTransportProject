using Domain.Entities;

namespace Application.Features.HowItWork.Queries.GetList
{
    public class GetListHowItWorkDto
    {
        public string HowItWorkId { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }

        public List<HowItWorkArticleDto> HowItWorkArticles { get; set; }
    }

    public class HowItWorkArticleDto
    {
        public int HowItWorkArticleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
