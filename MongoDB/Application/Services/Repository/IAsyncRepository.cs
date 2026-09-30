using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Persistence.Paginate;

namespace Application.Services.Repository
{
    public interface IAsyncRepository<T> where T : class
    {
        public Task CreateAsync(T t);
        public Task UpdateAsync(T t, string Id);
        public Task<T> GetByIdAsync(string Id);
        public Task<Paginate<T>> GetListAsync(int size, int page, Expression<Func<T, bool>>? predicate=null);
        public Task DeleteAsync(string Id);

        public Task<List<T>> OnlyListAsync(Expression<Func<T, bool>>? predicate = null);

    }
}
