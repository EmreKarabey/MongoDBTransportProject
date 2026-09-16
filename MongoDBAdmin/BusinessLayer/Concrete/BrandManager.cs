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
    public class BrandManager : IBrandService
    {
        private readonly IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        public async Task CreateAsync(Brand t)
        {
            await _brandDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
           await _brandDal.DeleteAsync(Id);
        }

        public async Task<Brand> GetByIdAsync(string Id)
        {
            return await _brandDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<Brand>> GetListAsync(int size, int page)
        {
           return await _brandDal.GetListAsync(size, page);
        }

        public async Task UpdateAsync(Brand t, string Id)
        {
           await _brandDal.UpdateAsync(t, Id);
        }
    }
}
