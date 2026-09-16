using Application.Services.Repository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Command.Create
{
    public class CreateCommentCommand : IRequest
    {
        public string FullName { get; set; }
        public string ImageURL { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
        public Guid AppUserId { get; set; }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Comment>(request);
            await _commentRepository.CreateAsync(entity);
        }
    }
}
