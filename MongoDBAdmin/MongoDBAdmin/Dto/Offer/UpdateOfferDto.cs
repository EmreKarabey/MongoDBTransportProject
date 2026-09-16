using Microsoft.AspNetCore.Http;

namespace MongoDBAdmin.Dto.Offer
{
    public class UpdateOfferDto
    {
        public string OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageURL { get; set; }
        public bool IsStatus { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}