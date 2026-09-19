using Application.Features.FAQ.Rules;
using Application.Services.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Command.Delete
{
    public class DeleteFAQCommand : IRequest
    {
        public string FaqId { get; set; }
    }

    public class DeleteFAQCommandHandler : IRequestHandler<DeleteFAQCommand>
    {
        private readonly IFAQRepository _faqRepository;
        private readonly FAQBusinessRules _faqBusinessRules;

        public DeleteFAQCommandHandler(IFAQRepository faqRepository, FAQBusinessRules faqBusinessRules)
        {
            _faqRepository = faqRepository;
            _faqBusinessRules = faqBusinessRules;
        }

        public async Task Handle(DeleteFAQCommand request, CancellationToken cancellationToken)
        {
            await _faqBusinessRules.FAQShouldExistWhenSelected(request.FaqId);

            await _faqRepository.DeleteAsync(request.FaqId);
        }
    }
}
