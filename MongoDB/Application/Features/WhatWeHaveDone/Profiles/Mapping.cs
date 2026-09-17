using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.WhatWeHaveDone.Command.Create;
using Application.Features.WhatWeHaveDone.Queries.GetList;
using AutoMapper;
using Domain.Entities;
using Persistence.Paginate;

namespace Application.Features.WhatWeHaveDone.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.WhatWeHaveDone, CreateWhatWeHaveDoneCommand>().ReverseMap();

            CreateMap<Domain.Entities.WhatWeHaveDone, GetListWhatWeHaveDoneDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.WhatWeHaveDone>, Paginate<GetListWhatWeHaveDoneDto>>().ReverseMap();
            
            CreateMap<Domain.Entities.WhatWeHaveDone, Application.Features.WhatWeHaveDone.Queries.GetActive.GetActiveListWhatWeHaveDoneDto>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.WhatWeHaveDone>, Paginate<Application.Features.WhatWeHaveDone.Queries.GetActive.GetActiveListWhatWeHaveDoneDto>>().ReverseMap();
        }
    }
}
