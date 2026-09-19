using Application.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Rules
{
    public class FAQBusinessRules
    {
        private readonly IFAQRepository _faqRepository;

        public FAQBusinessRules(IFAQRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public async Task FAQShouldExistWhenSelected(string id)
        {
            var result = await _faqRepository.GetByIdAsync(id);
            if (result == null) throw new Exception(Constants.FAQMessages.FAQNotFound);
        }
    }
}
