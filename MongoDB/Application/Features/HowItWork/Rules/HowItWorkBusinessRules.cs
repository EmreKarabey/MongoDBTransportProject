using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;

namespace Application.Features.HowItWork.Rules
{
    public class HowItWorkBusinessRules
    {
        private readonly IHowItWorkRepository _howItWorkRepository;

        public HowItWorkBusinessRules(IHowItWorkRepository howItWorkRepository)
        {
            _howItWorkRepository = howItWorkRepository;
        }
    }
}
