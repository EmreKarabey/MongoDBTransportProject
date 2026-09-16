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

        public Paginate()
        {
            Items = new List<T>();
            Size = 0;
            Page = 0;
        }

        public Paginate(List<T> ıtems, int size, int page)
        {
            Items = ıtems;
            Size = size;
            Page = page;
        }
    }
}
