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
    public class GetInTouchManager : IGetInTouchService
    {
        private readonly IGetInTouchDal _getInTouchDal;

        public GetInTouchManager(IGetInTouchDal getInTouchDal)
        {
            _getInTouchDal = getInTouchDal;
        }

        public async Task CreateAsync(GetInTouch t)
        {
            await _getInTouchDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
            await _getInTouchDal.DeleteAsync(Id);
        }

        public async Task<GetInTouch> GetByIdAsync(string Id)
        {
            return await _getInTouchDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<GetInTouch>> GetListAsync(int size, int page)
        {
            return await _getInTouchDal.GetListAsync(size, page);
        }

        public async Task<List<GetInTouch>> GetListAsync()
        {
            return await _getInTouchDal.GetListAsync();
        }

        public async Task UpdateAsync(GetInTouch t, string Id)
        {
            await _getInTouchDal.UpdateAsync(t, Id);
        }
    }
}
