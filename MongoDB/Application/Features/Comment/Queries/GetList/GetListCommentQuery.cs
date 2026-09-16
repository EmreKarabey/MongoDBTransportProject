using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Queries.GetList
{
    public class GetListCommentQuery : IRequest<Paginate<GetListCommentDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetListCommentHandler : IRequestHandler<GetListCommentQuery, Paginate<GetListCommentDto>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public GetListCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListCommentDto>> Handle(GetListCommentQuery request, CancellationToken cancellationToken)
        {
            var list = await _commentRepository.GetListAsync(request.pageResult.Size, request.pageResult.Index);
            var mapper = _mapper.Map<Paginate<GetListCommentDto>>(list);
            return mapper;
        }
    }
}
