using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.HowItWork.Queries.GetList
{
    public class GetListHowItWorkQuery : IRequest<Paginate<GetListHowItWorkDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetListHowItWorkHandler : IRequestHandler<GetListHowItWorkQuery, Paginate<GetListHowItWorkDto>>
    {
        private readonly IHowItWorkRepository _howItWorkRepository;
        private readonly IMapper _mapper;

        public GetListHowItWorkHandler(IHowItWorkRepository howItWorkRepository, IMapper mapper)
        {
            _howItWorkRepository = howItWorkRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListHowItWorkDto>> Handle(GetListHowItWorkQuery request, CancellationToken cancellationToken)
        {
            var list = await _howItWorkRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index);

            var mapper = _mapper.Map<Paginate<GetListHowItWorkDto>>(list);

            return mapper;
        }
    }
}
