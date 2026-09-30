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
    public class OfferManager : IOfferService
    {
        private readonly IOfferDal _offerDal;

        public OfferManager(IOfferDal offerDal)
        {
            _offerDal = offerDal;
        }

        public async Task CreateAsync(Offer t)
        {
            await _offerDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
           await _offerDal.DeleteAsync(Id);
        }

        public async Task<Offer> GetByIdAsync(string Id)
        {
            return await _offerDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<Offer>> GetListAsync(int size, int page)
        {
            return await _offerDal.GetListAsync(size, page);
        }

        public async Task<List<Offer>> GetListAsync()
        {
            return await _offerDal.GetListAsync();
        }

        public async Task UpdateAsync(Offer t, string Id)
        {
           await _offerDal.UpdateAsync(t, Id);
        }
    }
}
