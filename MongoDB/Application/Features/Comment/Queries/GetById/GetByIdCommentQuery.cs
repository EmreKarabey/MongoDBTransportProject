using Application.Services.Repository;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Queries.GetById
{
    public class GetByIdCommentQuery : IRequest<GetByIdCommentDto>
    {
        public string CommentId { get; set; }
    }

    public class GetByIdCommentHandler : IRequestHandler<GetByIdCommentQuery, GetByIdCommentDto>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetByIdCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<GetByIdCommentDto> Handle(GetByIdCommentQuery request, CancellationToken cancellationToken)
        {
            var value = await _commentRepository.GetByIdAsync(request.CommentId);
            return _mapper.Map<GetByIdCommentDto>(value);
        }
    }
}
