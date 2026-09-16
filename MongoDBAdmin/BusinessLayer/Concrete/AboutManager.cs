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
    public class AboutManager : IAboutService
    {
        private readonly IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public async Task CreateAsync(About t)
        {
            await _aboutDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
            await _aboutDal.DeleteAsync(Id);
        }

        public async Task<About> GetByIdAsync(string Id)
        {
            return await _aboutDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<About>> GetListAsync(int size, int page)
        {
            return await _aboutDal.GetListAsync(size, page);
        }

        public async Task UpdateAsync(About t, string Id)
        {
            await _aboutDal.UpdateAsync (t, Id);
        }

        public async Task<bool> AnyActive()
        {
            return await _aboutDal.AnyActive();
        }
    }
}
