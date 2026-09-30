using BusinessLayer.Abstract;
using DataAcessLayer.Abstract;
using EntityLayer.Entities;
using EntityLayer.Paginate;

namespace BusinessLayer.Concrete
{
    public class HowItWorkManager : IHowItWorkService
    {
        private readonly IHowItWorkDal _howItWorkDal;

        public HowItWorkManager(IHowItWorkDal howItWorkDal)
        {
            _howItWorkDal = howItWorkDal;
        }

        public async Task CreateAsync(HowItWork t)
        {
            await _howItWorkDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await _howItWorkDal.GetByIdAsync(id);
            await _howItWorkDal.DeleteAsync(entity.HowItWorkId);
        }

        public async Task<HowItWork> GetByIdAsync(string id)
        {
            return await _howItWorkDal.GetByIdAsync(id);
        }

        public async Task<Paginate<HowItWork>> GetListAsync(int size = 10, int index = 1)
        {
            return await _howItWorkDal.GetListAsync( size, index);
        }

        public async Task<List<HowItWork>> GetListAsync()
        {
            return await _howItWorkDal.GetListAsync();
        }

        public async Task UpdateAsync(HowItWork t, string id)
        {
            await _howItWorkDal.UpdateAsync(t, id);
        }
    }
}
