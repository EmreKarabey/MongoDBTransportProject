using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.PageResult;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.WhatWeHaveDone.Queries.GetActive
{
    public class GetActiveListWhatWeHaveDoneQuery:IRequest<Paginate<GetActiveListWhatWeHaveDoneDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetActiveListWhatWeHaveDoneHandler : IRequestHandler<GetActiveListWhatWeHaveDoneQuery, Paginate<GetActiveListWhatWeHaveDoneDto>>
    {
        private readonly IWhatWeHaveDoneRepository _whatWeHaveDoneRepository;
        private readonly IMapper _mapper;

        public GetActiveListWhatWeHaveDoneHandler(IWhatWeHaveDoneRepository whatWeHaveDoneRepository, IMapper mapper)
        {
            _whatWeHaveDoneRepository = whatWeHaveDoneRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetActiveListWhatWeHaveDoneDto>> Handle(GetActiveListWhatWeHaveDoneQuery request, CancellationToken cancellationToken)
        {
            var list = await _whatWeHaveDoneRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index, predicate: n => n.IsActive);

            var result = _mapper.Map<Paginate<GetActiveListWhatWeHaveDoneDto>>(list);

            return result;
        }
    }
}
