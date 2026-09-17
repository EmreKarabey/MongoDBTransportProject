using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;

namespace Application.Features.WhatWeHaveDone.Rules
{
    public class WhatWeHaveDoneBusinessRules
    {
        private readonly IWhatWeHaveDoneRepository _whatWeHaveDoneRepository;

        public WhatWeHaveDoneBusinessRules(IWhatWeHaveDoneRepository whatWeHaveDoneRepository)
        {
            _whatWeHaveDoneRepository = whatWeHaveDoneRepository;
        }
    }
}
