using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace EntityLayer.Entities
{
    [BsonIgnoreExtraElements]
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? ImageURL { get; set; }
        public int? Code { get; set; }
        public DateTime? CodeExpiryTime { get; set; }
    }
}
