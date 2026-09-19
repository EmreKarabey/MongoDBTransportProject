using Application.Features.FAQ.Rules;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Command.Create
{
    public class CreateFAQCommand : IRequest
    {
        public string Question { get; set; }
        public string Description { get; set; }
    }

    public class CreateFAQCommandHandler : IRequestHandler<CreateFAQCommand>
    {
        private readonly IFAQRepository _faqRepository;
        private readonly IMapper _mapper;
        private readonly FAQBusinessRules _faqBusinessRules;

        public CreateFAQCommandHandler(IFAQRepository faqRepository, IMapper mapper, FAQBusinessRules faqBusinessRules)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
            _faqBusinessRules = faqBusinessRules;
        }

        public async Task Handle(CreateFAQCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.FAQ>(request);
            entity.IsActive = true;
            await _faqRepository.CreateAsync(entity);
        }
    }
}
