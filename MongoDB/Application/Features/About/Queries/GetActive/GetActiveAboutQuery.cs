using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.About.Queries.GetActive
{
    public class GetActiveAboutQuery:IRequest<Paginate<GetActiveAboutDto>>
    {
       
    }
    public class GetActiveAboutHandler : IRequestHandler<GetActiveAboutQuery, Paginate<GetActiveAboutDto>>
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IMapper _mapper;
        public GetActiveAboutHandler(IAboutRepository aboutRepository, IMapper mapper)
        {
            _aboutRepository = aboutRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetActiveAboutDto>> Handle(GetActiveAboutQuery request, CancellationToken cancellationToken)
        {
            var active = await _aboutRepository.GetListAsync(1,1,predicate:n=>n.IsActive);

            var mapper = _mapper.Map<Paginate<GetActiveAboutDto>>(active);

            return mapper;
        }
    }
}
