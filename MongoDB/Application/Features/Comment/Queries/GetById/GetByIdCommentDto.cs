using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Comment.Queries.GetById
{
    public class GetByIdCommentDto
    {
        public string CommentId { get; set; }
        public string FullName { get; set; }
        public string ImageURL { get; set; }
        public string Content { get; set; }
        public string Details { get; set; }
        public Guid AppUserId { get; set; }
    }
}
