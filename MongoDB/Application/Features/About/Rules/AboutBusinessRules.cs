using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;

namespace Application.Features.About.Rules
{
    public class AboutBusinessRules
    {
        private readonly IAboutRepository _aboutRepository;

        public AboutBusinessRules(IAboutRepository aboutRepository)
        {
            _aboutRepository = aboutRepository;
        }
    }
}
