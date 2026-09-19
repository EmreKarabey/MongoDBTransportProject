using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.FAQ.Queries.GetById
{
    public class GetByIdFAQDto
    {
        public string FaqId { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } 
    }
}
