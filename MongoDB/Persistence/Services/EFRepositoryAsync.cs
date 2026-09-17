using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Services.Repository;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Persistence.Paginate;

namespace Persistence.Services
{
    public class EFRepositoryAsync<T> : IAsyncRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public EFRepositoryAsync(IDataBaseSettings _dataBaseSettings)
        {
            var client = new MongoClient(_dataBaseSettings.ConnectionString);

            var database = client.GetDatabase(_dataBaseSettings.DatabaseName);

            var collectionName = typeof(T).Name;

            _collection = database.GetCollection<T>(collectionName + "s");

        }

        public async Task CreateAsync(T t)
        {
            await _collection.InsertOneAsync(t);
        }

        public async Task DeleteAsync(string id)
        {
            var builder = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(builder);
        }

        public async Task<T> GetByIdAsync(string Id)
        {
            var builder = Builders<T>.Filter.Eq("_id", Id);

            var find = await _collection.Find(builder).FirstOrDefaultAsync();

            return find;
        }

        public async Task<Paginate<T>> GetListAsync(int size, int page)
        {
            var skipItem = (page - 1) * size;

            var totalCount = (int)await _collection.CountDocumentsAsync(Builders<T>.Filter.Empty);
            var list = await _collection.AsQueryable().Skip(skipItem).Take(size).ToListAsync();

            var paginate = new Paginate<T>(list, size, page, totalCount);

            return paginate;
        }

        public async Task<Paginate<T>> GetListAsync(int size, int page, Expression<Func<T, bool>>? predicate=null)
        {
            var skipItem = (page - 1) * size;

            var query = _collection.AsQueryable();

            if (predicate != null) query = query.Where(predicate);
            
            var filter = predicate != null ? Builders<T>.Filter.Where(predicate) : Builders<T>.Filter.Empty;
            var totalCount = (int)await _collection.CountDocumentsAsync(filter);

            var list = await query.Skip(skipItem).Take(size).ToListAsync();

            if (!list.Any()) list = new List<T>();

            var paginate = new Paginate<T>(list, size, page, totalCount);

            return paginate;
        }

        public async Task UpdateAsync(T t, string Id)
        {
            var builder = Builders<T>.Filter.Eq("_id", Id);

            await _collection.FindOneAndReplaceAsync(builder, t);
        }
    }
}
