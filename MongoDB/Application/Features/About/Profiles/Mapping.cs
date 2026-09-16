using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.About.Queries.GetActive;
using Application.Features.About.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Persistence.Paginate;

namespace Application.Features.About.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.About, GetListAboutDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.About>, Paginate<GetListAboutDto>>().ReverseMap();

            CreateMap<GetActiveAboutDto, Domain.Entities.About>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.About>, Paginate<GetActiveAboutDto>>().ReverseMap();
        }
    }
}
