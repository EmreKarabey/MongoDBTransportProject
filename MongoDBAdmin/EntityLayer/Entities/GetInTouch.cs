using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EntityLayer.Entities
{
    public class GetInTouch
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string GetInTouchId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }

        public List<GetInTouchArticle> GetInTouchArticles { get; set; }
    }

    public class GetInTouchArticle
    {
        public int GetInTouchArticleId { get; set; }
        public string IconURL { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
