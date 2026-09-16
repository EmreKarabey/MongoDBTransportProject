using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.PageResult;
using Application.Services.Repository;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Brand.Queries.GetList
{
    public class GetListBrandQuery : IRequest<Paginate<GetListBrandDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetListBrandHandler : IRequestHandler<GetListBrandQuery, Paginate<GetListBrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetListBrandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListBrandDto>> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
        {
            var list = await _brandRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index);

            var result = _mapper.Map<Paginate<GetListBrandDto>>(list);

            return result;
        }
    }
}