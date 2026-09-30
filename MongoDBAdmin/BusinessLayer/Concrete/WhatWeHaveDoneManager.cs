using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Abstract;
using DataAcessLayer.Abstract;
using EntityLayer.Entities;

using EntityLayer.Paginate;

namespace BusinessLayer.Concrete
{
    public class WhatWeHaveDoneManager : IWhatWeHaveDoneService
    {
        private readonly IWhatWeHaveDoneDal _whatWeHaveDoneDal;

        public WhatWeHaveDoneManager(IWhatWeHaveDoneDal whatWeHaveDoneDal)
        {
            _whatWeHaveDoneDal = whatWeHaveDoneDal;
        }

        public async Task CreateAsync(WhatWeHaveDone entity)
        {
            await _whatWeHaveDoneDal.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _whatWeHaveDoneDal.DeleteAsync(id);
        }

        public async Task<WhatWeHaveDone> GetByIdAsync(string id)
        {
            return await _whatWeHaveDoneDal.GetByIdAsync(id);
        }

        public async Task<Paginate<WhatWeHaveDone>> GetListAsync(int limit, int skip)
        {
            return await _whatWeHaveDoneDal.GetListAsync(limit, skip);
        }

        public async Task<List<WhatWeHaveDone>> GetListAsync()
        {
            return await _whatWeHaveDoneDal.GetListAsync();
        }

        public async Task UpdateAsync(WhatWeHaveDone entity, string id)
        {
            await _whatWeHaveDoneDal.UpdateAsync(entity, id);
        }
    }
}
