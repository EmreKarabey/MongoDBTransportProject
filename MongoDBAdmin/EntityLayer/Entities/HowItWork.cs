using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EntityLayer.Entities
{
    public class HowItWork
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string HowItWorkId { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }

        public List<HowItWorkArticle> HowItWorkArticles { get; set; } = new();
    }

    public class HowItWorkArticle
    {
        public int HowItWorkArticleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }
}
