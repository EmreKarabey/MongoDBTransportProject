using EntityLayer.Paginate;

namespace BusinessLayer.Abstract
{
    public interface IGenericService<T> where T : class
    {
        public Task CreateAsync(T t);
        public Task UpdateAsync(T t, string Id);
        public Task<T> GetByIdAsync(string Id);
        public Task<Paginate<T>> GetListAsync(int size, int page);
        public Task DeleteAsync(string Id);
    }
}
