using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Brand.Queries.GetActive;
using Application.Features.Brand.Queries.GetList;
using Application.Features.Slider.Command.Create;
using Application.Features.Slider.Queries.GetList;
using AutoMapper;
using Persistence.Paginate;

namespace Application.Features.Brand.Profiles
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Brand, GetListBrandDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Brand>, Paginate<GetListBrandDto>>().ReverseMap();

            CreateMap<Domain.Entities.Brand, GetActiveListBrandDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Brand>, Paginate<GetActiveListBrandDto>>().ReverseMap();

        }
    }
}
