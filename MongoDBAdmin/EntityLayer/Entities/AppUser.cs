using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace EntityLayer.Entities
{
    [CollectionName("Users")]
    public class AppUser : MongoIdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string ImageURL { get; set; }
    }
}
