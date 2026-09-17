using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Paginate
{
    public class Paginate<T> where T : class
    {


        public List<T> Items { get; set; }
        public int Size { get; set; }
        public int Page { get; set; }

        public int TotalCount { get; set; }
        public int TotalPages => Size > 0 ? (int)Math.Ceiling(TotalCount / (double)Size) : 0;
        public bool HasNext => Page < TotalPages;

        public Paginate()
        {
            Items = new List<T>();
            Size = 0;
            Page = 0;
            TotalCount = 0;
        }

        public Paginate(List<T> items, int size, int page, int totalCount)
        {
            Items = items;
            Size = size;
            Page = page;
            TotalCount = totalCount;
        }
    }
}
