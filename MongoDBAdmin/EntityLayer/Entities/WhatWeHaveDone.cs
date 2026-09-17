using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EntityLayer.Entities
{
    public class WhatWeHaveDone
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string WhatWeHaveDoneId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool IsActive { get; set; }
    }
}
