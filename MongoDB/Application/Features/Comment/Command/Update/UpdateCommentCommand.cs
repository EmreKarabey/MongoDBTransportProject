using Application.Services.Repository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Command.Update
{
    public class UpdateCommentCommand : IRequest
    {
        public string CommentId { get; set; }
        public string FullName { get; set; }
        public string ImageURL { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
        public Guid AppUserId { get; set; }
    }

    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public UpdateCommentCommandHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Comment>(request);
            await _commentRepository.UpdateAsync(entity, request.CommentId);
        }
    }
}
