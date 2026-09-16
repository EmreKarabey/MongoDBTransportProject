using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Features.Brand.Queries.GetActive;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Offer.Queries.GetActiveList
{
    public class GetActiveOfferListQuery:IRequest<Paginate<GetActiveOfferListDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetActiveOfferListHandler : IRequestHandler<GetActiveOfferListQuery, Paginate<GetActiveOfferListDto>>
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public GetActiveOfferListHandler(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetActiveOfferListDto>> Handle(GetActiveOfferListQuery request, CancellationToken cancellationToken)
        {
            var list = await _offerRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index, predicate: n => n.IsStatus);

            var result = _mapper.Map<Paginate<GetActiveOfferListDto>>(list);

            return result;
        }
    }
}
