using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace EntityLayer.Entities
{
    [BsonIgnoreExtraElements]
    [CollectionName("Roles")]
    public class AppRole : MongoIdentityRole<Guid>
    {
    }
}
