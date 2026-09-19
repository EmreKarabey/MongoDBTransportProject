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
    public class FAQManager : IFAQService
    {
        private readonly IFAQDal _faqDal;

        public FAQManager(IFAQDal faqDal)
        {
            _faqDal = faqDal;
        }

        public async Task CreateAsync(FAQ entity)
        {
            await _faqDal.CreateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _faqDal.DeleteAsync(id);
        }

        public async Task<FAQ> GetByIdAsync(string id)
        {
            return await _faqDal.GetByIdAsync(id);
        }

        public async Task<Paginate<FAQ>> GetListAsync(int limit, int skip)
        {
            return await _faqDal.GetListAsync(limit, skip);
        }

        public async Task UpdateAsync(FAQ entity, string id)
        {
            await _faqDal.UpdateAsync(entity, id);
        }
    }
}
