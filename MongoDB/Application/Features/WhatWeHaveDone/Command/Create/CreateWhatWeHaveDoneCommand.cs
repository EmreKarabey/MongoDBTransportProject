using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using AutoMapper;
using MediatR;

namespace Application.Features.WhatWeHaveDone.Command.Create
{
    public class CreateWhatWeHaveDoneCommand:IRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }

    public class CreateWhatWeHaveDoneCommandHandler : IRequestHandler<CreateWhatWeHaveDoneCommand>
    {
        private readonly IWhatWeHaveDoneRepository _whatWeHaveDoneRepository;
        private readonly IMapper _mapper;

        public CreateWhatWeHaveDoneCommandHandler(IWhatWeHaveDoneRepository whatWeHaveDoneRepository, IMapper mapper)
        {
            _whatWeHaveDoneRepository = whatWeHaveDoneRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreateWhatWeHaveDoneCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.WhatWeHaveDone>(request);

            await _whatWeHaveDoneRepository.CreateAsync(entity);
        }
    }
}
