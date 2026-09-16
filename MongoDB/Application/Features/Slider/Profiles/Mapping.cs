using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Slider.Command.Create;
using Application.Features.Slider.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Persistence.Paginate;

namespace Application.Features.Slider.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Slider, CreateSliderCommand>().ReverseMap();

            CreateMap<Domain.Entities.Slider, GetListSliderDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Slider>, Paginate<GetListSliderDto>>().ReverseMap();

        }
    }
}
