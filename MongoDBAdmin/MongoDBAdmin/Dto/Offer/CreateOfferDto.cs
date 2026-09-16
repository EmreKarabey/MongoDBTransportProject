using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.Offer
{
    public class CreateOfferDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public bool IsStatus { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
