using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Offer.Queries.GetActiveList
{
    public class GetActiveOfferListDto
    {
        public string OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool IsStatus { get; set; }
    }
}
