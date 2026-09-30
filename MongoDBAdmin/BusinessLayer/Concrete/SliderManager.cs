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
    public class SliderManager : ISliderService
    {
        private readonly ISliderDal _sliderDal;

        public SliderManager(ISliderDal sliderDal)
        {
            _sliderDal = sliderDal;
        }

        public async Task CreateAsync(Slider t)
        {
            await _sliderDal.CreateAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
            await _sliderDal.DeleteAsync(Id);
        }

        public async Task<Slider> GetByIdAsync(string Id)
        {
            return await _sliderDal.GetByIdAsync(Id);
        }

        public async Task<Paginate<Slider>> GetListAsync(int size, int page)
        {
           return await _sliderDal.GetListAsync(size, page);
        }

        public async Task<List<Slider>> GetListAsync()
        {
            return await _sliderDal.GetListAsync();
        }

        public async Task UpdateAsync(Slider t, string Id)
        {
            await _sliderDal.UpdateAsync(t, Id);
        }
    }
}
