using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataAcessLayer.Abstract;
using DataAcessLayer.Repository;
using EntityLayer.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAcessLayer.Concrete
{
    public class EFAboutDal : GenericRepository<About>, IAboutDal
    {
        public EFAboutDal(IDatabaseSettings databaseSettings) : base(databaseSettings)
        {
        }

        public async Task<bool> AnyActive()
        {
            var any = Builders<About>.Filter.Eq(x => x.IsActive, true);

            var count = await _collection.CountDocumentsAsync(any);

            if (count > 0) return true;
            return false;
        }
    }
}
