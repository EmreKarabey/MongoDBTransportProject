using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataAcessLayer.Abstract;
using EntityLayer.Paginate;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace DataAcessLayer.Repository
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public GenericRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var dataBase = client.GetDatabase(databaseSettings.DatabaseName);

            var collectionName = typeof(T).Name;

            _collection = dataBase.GetCollection<T>(collectionName + "s");
        }

        public async Task CreateAsync(T t)
        {
            await _collection.InsertOneAsync(t);
        }

        public async Task DeleteAsync(string Id)
        {
            if (!ObjectId.TryParse(Id, out var objectId)) return;
            var builder = Builders<T>.Filter.Eq("_id", objectId);
            await _collection.DeleteOneAsync(builder);
        }

        public async Task<T> GetByIdAsync(string Id)
        {
            if (!ObjectId.TryParse(Id, out var objectId)) return null;

            var builder = Builders<T>.Filter.Eq("_id", objectId);

            var find = await _collection.Find(builder).FirstOrDefaultAsync();

            return find;
        }

        public async Task<EntityLayer.Paginate.Paginate<T>> GetListAsync(int size, int page)
        {
            var skipItem = (page - 1) * size;
            var totalCount = (int)await _collection.CountDocumentsAsync(Builders<T>.Filter.Empty);

            var list = await _collection.AsQueryable().Skip(skipItem).Take(size).ToListAsync();

            if (!list.Any()) list = new List<T>();

            var paginate = new Paginate<T>(list, size, page, totalCount);

            return paginate;
        }

        public async Task UpdateAsync(T t, string Id)
        {
            if (!ObjectId.TryParse(Id, out var objectId)) return;
            var builder = Builders<T>.Filter.Eq("_id", objectId);

            await _collection.FindOneAndReplaceAsync(builder, t);
        }
    }
}
