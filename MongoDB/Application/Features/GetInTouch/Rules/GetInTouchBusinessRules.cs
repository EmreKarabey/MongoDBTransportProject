using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;

namespace Application.Features.GetInTouch.Rules
{
    public class GetInTouchBusinessRules
    {
        private readonly IGetInTouchRepository _getInTouchRepository;

        public GetInTouchBusinessRules(IGetInTouchRepository getInTouchRepository)
        {
            _getInTouchRepository = getInTouchRepository;
        }
    }
}
