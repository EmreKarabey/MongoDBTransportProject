using EntityLayer.Entities;

namespace MongoDBAdmin.Dto.Shipment
{
    public class UpdateShipmentDto
    {
        public string ShipmentId { get; set; }
        public string TrackingNumber { get; set; }

        public string SenderName { get; set; }

        public Guid ReceiverUserId { get; set; }
        public string ReceiverName { get; set; }

        public string OriginCity { get; set; }
        public string DestinationCity { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CurrentStatus { get; set; }

        public bool IsActive { get; set; } = true;

        public List<UpdateShipmentTrackingDto> Trackings { get; set; } = new List<UpdateShipmentTrackingDto>();
    }

    public class UpdateShipmentTrackingDto
    {
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string TrackingStatus { get; set; }
    }
}
