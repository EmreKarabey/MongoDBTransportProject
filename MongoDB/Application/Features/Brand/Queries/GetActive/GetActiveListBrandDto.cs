using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Brand.Queries.GetActive
{
    public class GetActiveListBrandDto
    {
        public string BrandId { get; set; }
        public string BrandName { get; set; }
        public string ImageURL { get; set; }
        public bool IsStatus { get; set; }
    }
}
