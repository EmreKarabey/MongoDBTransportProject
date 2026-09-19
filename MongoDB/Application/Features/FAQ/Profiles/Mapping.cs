using Application.Features.FAQ.Command.Create;
using Application.Features.FAQ.Command.Update;
using Application.Features.FAQ.Queries.GetActiveList;
using Application.Features.FAQ.Queries.GetById;
using Application.Features.FAQ.Queries.GetList;
using AutoMapper;
using Persistence.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.FAQ, CreateFAQCommand>().ReverseMap();
            CreateMap<Domain.Entities.FAQ, UpdateFAQCommand>().ReverseMap();
            
            CreateMap<Domain.Entities.FAQ, GetListFAQDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.FAQ>, Paginate<GetListFAQDto>>().ReverseMap();
            
            CreateMap<Domain.Entities.FAQ, GetByIdFAQDto>().ReverseMap();

            CreateMap<Domain.Entities.FAQ, GetActiveListFAQDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.FAQ>, Paginate<GetActiveListFAQDto>>().ReverseMap();
        }
    }
}
