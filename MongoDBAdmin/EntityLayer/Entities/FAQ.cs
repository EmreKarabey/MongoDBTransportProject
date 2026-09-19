using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EntityLayer.Entities
{
    public class FAQ
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string FaqId { get; set; }
        public string Question { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
