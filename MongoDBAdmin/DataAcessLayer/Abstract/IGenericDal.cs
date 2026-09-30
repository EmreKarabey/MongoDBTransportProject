using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Paginate;

namespace DataAcessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {
        public Task CreateAsync(T t);
        public Task UpdateAsync(T t, string Id);
        public Task<T> GetByIdAsync(string Id);
        public Task<Paginate<T>> GetListAsync(int size, int page);
        public Task DeleteAsync(string Id);
        public Task<List<T>> GetListAsync();
    }
}
