using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Queries.GetList
{
    public class GetListFAQQuery : IRequest<Paginate<GetListFAQDto>>
    {
        public int Page { get; set; }
        public int Size { get; set; }
    }

    public class GetListFAQQueryHandler : IRequestHandler<GetListFAQQuery, Paginate<GetListFAQDto>>
    {
        private readonly IFAQRepository _faqRepository;
        private readonly IMapper _mapper;

        public GetListFAQQueryHandler(IFAQRepository faqRepository, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListFAQDto>> Handle(GetListFAQQuery request, CancellationToken cancellationToken)
        {
            var faqs = await _faqRepository.GetListAsync(size: request.Size, page: request.Page);
            var mappedFaqs = _mapper.Map<Paginate<GetListFAQDto>>(faqs);
            return mappedFaqs;
        }
    }
}
