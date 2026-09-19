using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.PageResult;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.FAQ.Queries.GetActiveList
{
    public class GetActiveListFAQQuery : IRequest<Paginate<GetActiveListFAQDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }
    public class GetActiveListFAQHandler : IRequestHandler<GetActiveListFAQQuery, Paginate<GetActiveListFAQDto>>
    {
        private readonly IFAQRepository _fAQRepository;
        private readonly IMapper __mapper;

        public GetActiveListFAQHandler(IFAQRepository fAQRepository, IMapper mapper)
        {
            _fAQRepository = fAQRepository;
            __mapper = mapper;
        }

        public async Task<Paginate<GetActiveListFAQDto>> Handle(GetActiveListFAQQuery request, CancellationToken cancellationToken)
        {
            var list = await _fAQRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index, predicate: n => n.IsActive);

            var mapperList = __mapper.Map<Paginate<GetActiveListFAQDto>>(list);
            return mapperList;
        }
    }
}
