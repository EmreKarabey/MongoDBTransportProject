using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.PageResult
{
    public class PageResult
    {


        public int Size { get; set; }
        public int Index { get; set; }

        public PageResult() { }

        public PageResult(int size, int ındex)
        {
            Size = size;
            Index = ındex;
        }
    }
}
