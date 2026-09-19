using Application.Features.FAQ.Rules;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Command.Update
{
    public class UpdateFAQCommand : IRequest
    {
        public string FaqId { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
    }

    public class UpdateFAQCommandHandler : IRequestHandler<UpdateFAQCommand>
    {
        private readonly IFAQRepository _faqRepository;
        private readonly IMapper _mapper;
        private readonly FAQBusinessRules _faqBusinessRules;

        public UpdateFAQCommandHandler(IFAQRepository faqRepository, IMapper mapper, FAQBusinessRules faqBusinessRules)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
            _faqBusinessRules = faqBusinessRules;
        }

        public async Task Handle(UpdateFAQCommand request, CancellationToken cancellationToken)
        {
            await _faqBusinessRules.FAQShouldExistWhenSelected(request.FaqId);

            var entity = await _faqRepository.GetByIdAsync(request.FaqId);
            _mapper.Map(request, entity);

            await _faqRepository.UpdateAsync(entity, request.FaqId);
        }
    }
}
