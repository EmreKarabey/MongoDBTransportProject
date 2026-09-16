using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Domain.Entities
{
    [CollectionName("Roles")]
    public class AppRole:MongoIdentityRole<Guid>
    {
    }
}
