using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.ToxicBert
{
    public interface IToxicBertService
    {
        public Task<List<ToxicScoreDto>> ToxicScore(string text);
    }
}
