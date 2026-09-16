using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.About.Queries.GetList
{
    public class GetListAboutQuery : IRequest<Paginate<GetListAboutDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetListAboutHandler : IRequestHandler<GetListAboutQuery, Paginate<GetListAboutDto>>
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IMapper _mapper;

        public GetListAboutHandler(IAboutRepository aboutRepository, IMapper mapper)
        {
            _aboutRepository = aboutRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListAboutDto>> Handle(GetListAboutQuery request, CancellationToken cancellationToken)
        {
            var list = await _aboutRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index);

            var mapper = _mapper.Map<Paginate<GetListAboutDto>>(list);

            return mapper;
        }
    }
}
