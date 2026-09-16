using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.HowItWork.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Persistence.Paginate;

namespace Application.Features.HowItWork.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.HowItWork, GetListHowItWorkDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.HowItWork>, Paginate<GetListHowItWorkDto>>().ReverseMap();
            CreateMap<Domain.Entities.HowItWorkArticle, HowItWorkArticleDto>().ReverseMap();
        }
    }
}
