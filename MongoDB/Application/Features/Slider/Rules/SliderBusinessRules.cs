using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;

namespace Application.Features.Slider.Rules
{
    public class SliderBusinessRules
    {
        private readonly ISliderRepository _sliderRepository;

        public SliderBusinessRules(ISliderRepository sliderRepository)
        {
            _sliderRepository = sliderRepository;
        }

       
    }
}
