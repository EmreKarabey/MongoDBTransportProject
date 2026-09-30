using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Paginate
{
    public class Paginate<T> where T : class
    {

        public List<T> List { get; set; }

        public int Size { get; set; }
        public int Page { get; set; }

        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)Size);
        public bool HasNext => Page < TotalPages;


        public Paginate()
        {
            List = new List<T>();
            Size = 0;
            Page = 0;
            TotalCount = 0;
        }

        public Paginate(List<T> list, int size, int page, int totalCount)
        {
            List = list;
            Size = size;
            Page = page;
            TotalCount = totalCount;
        }
    }
}
