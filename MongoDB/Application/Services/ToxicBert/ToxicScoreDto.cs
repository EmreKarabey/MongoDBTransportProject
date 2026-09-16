using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ToxicBert
{
    public class ToxicScoreDto
    {
        public string Label { get; set; }
        public double Score { get; set; }
    }
}
