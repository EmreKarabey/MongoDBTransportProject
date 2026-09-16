using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using AutoMapper;
using MediatR;

namespace Application.Features.Slider.Command.Create
{
    public class CreateSliderCommand:IRequest
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }

    public class CreateSliderCommandHandler : IRequestHandler<CreateSliderCommand>
    {
        private readonly ISliderRepository _sliderRepository;
        private readonly IMapper _mapper;

        public CreateSliderCommandHandler(ISliderRepository sliderRepository, IMapper mapper)
        {
            _sliderRepository = sliderRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreateSliderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Slider>(request);

            await _sliderRepository.CreateAsync(entity);
        }
    }
}
