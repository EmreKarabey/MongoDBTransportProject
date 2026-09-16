using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Features.Brand.Queries.GetList;
using Application.PageResult;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Brand.Queries.GetActive
{
    public class GetActiveListBrandQuery:IRequest<Paginate<GetActiveListBrandDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetActiveListBrandHandler : IRequestHandler<GetActiveListBrandQuery, Paginate<GetActiveListBrandDto>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public GetActiveListBrandHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetActiveListBrandDto>> Handle(GetActiveListBrandQuery request, CancellationToken cancellationToken)
        {
            var list = await _brandRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index,predicate:n=>n.IsStatus);

            var result = _mapper.Map<Paginate<GetActiveListBrandDto>>(list);

            return result;
        }
    }
}
