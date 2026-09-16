using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Brand.Queries.GetActive;
using Application.Features.Offer.Queries.GetActiveList;
using AutoMapper;
using Persistence.Paginate;

namespace Application.Features.Offer.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Offer, GetActiveOfferListDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Offer>, Paginate<GetActiveOfferListDto>>().ReverseMap();
        }
    }
}
