using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.GetInTouch.Queries.GetList
{
    public class GetListGetInTouchQuery : IRequest<Paginate<GetListGetInTouchDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetListGetInTouchHandler : IRequestHandler<GetListGetInTouchQuery, Paginate<GetListGetInTouchDto>>
    {
        private readonly IGetInTouchRepository _getInTouchRepository;
        private readonly IMapper _mapper;

        public GetListGetInTouchHandler(IGetInTouchRepository getInTouchRepository, IMapper mapper)
        {
            _getInTouchRepository = getInTouchRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListGetInTouchDto>> Handle(GetListGetInTouchQuery request, CancellationToken cancellationToken)
        {
            var list = await _getInTouchRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index);

            var mapper = _mapper.Map<Paginate<GetListGetInTouchDto>>(list);

            return mapper;
        }
    }
}
