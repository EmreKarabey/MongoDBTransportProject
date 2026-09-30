using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Shipment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ShipmentId { get; set; }
        public string TrackingNumber { get; set; }

        public Guid SenderUserId { get; set; }
        public string SenderName { get; set; }


        public Guid ReceiverUserId { get; set; }
        public string ReceiverName { get; set; }

        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CurrentStatus { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ShipmentTracking> Trackings { get; set; }
    }
}
