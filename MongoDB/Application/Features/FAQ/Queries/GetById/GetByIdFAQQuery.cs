using Application.Features.FAQ.Rules;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Queries.GetById
{
    public class GetByIdFAQQuery : IRequest<GetByIdFAQDto>
    {
        public string FaqId { get; set; }
    }

    public class GetByIdFAQQueryHandler : IRequestHandler<GetByIdFAQQuery, GetByIdFAQDto>
    {
        private readonly IFAQRepository _faqRepository;
        private readonly IMapper _mapper;
        private readonly FAQBusinessRules _faqBusinessRules;

        public GetByIdFAQQueryHandler(IFAQRepository faqRepository, IMapper mapper, FAQBusinessRules faqBusinessRules)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
            _faqBusinessRules = faqBusinessRules;
        }

        public async Task<GetByIdFAQDto> Handle(GetByIdFAQQuery request, CancellationToken cancellationToken)
        {
            await _faqBusinessRules.FAQShouldExistWhenSelected(request.FaqId);

            var faq = await _faqRepository.GetByIdAsync(request.FaqId);
            var mappedFaq = _mapper.Map<GetByIdFAQDto>(faq);
            return mappedFaq;
        }
    }
}
