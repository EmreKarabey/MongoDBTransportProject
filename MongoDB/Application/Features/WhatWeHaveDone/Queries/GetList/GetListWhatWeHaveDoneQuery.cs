using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.WhatWeHaveDone.Queries.GetList
{
    public class GetListWhatWeHaveDoneQuery:IRequest<Paginate<GetListWhatWeHaveDoneDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }
    
    public class GetListWhatWeHaveDoneHandler : IRequestHandler<GetListWhatWeHaveDoneQuery, Paginate<GetListWhatWeHaveDoneDto>>
    {
        private readonly IWhatWeHaveDoneRepository _whatWeHaveDoneRepository;
        private readonly IMapper _mapper;

        public GetListWhatWeHaveDoneHandler(IWhatWeHaveDoneRepository whatWeHaveDoneRepository, IMapper mapper)
        {
            _whatWeHaveDoneRepository = whatWeHaveDoneRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListWhatWeHaveDoneDto>> Handle(GetListWhatWeHaveDoneQuery request, CancellationToken cancellationToken)
        {
            var list = await _whatWeHaveDoneRepository.GetListAsync(request.pageResult.Size,request.pageResult.Index);

            var mapper = _mapper.Map<Paginate<GetListWhatWeHaveDoneDto>>(list);

            return mapper;
        }
    }
}
