using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.GetInTouch.Queries.GetList;
using AutoMapper;
using Persistence.Paginate;

namespace Application.Features.GetInTouch.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.GetInTouch, GetListGetInTouchDto>().ReverseMap();
            CreateMap<Domain.Entities.GetInTouchArticle, GetListGetInTouchArticleDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.GetInTouch>, Paginate<GetListGetInTouchDto>>().ReverseMap();
        }
    }
}
