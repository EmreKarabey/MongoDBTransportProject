using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Features.Shipment.Queries.GetFiltreStatus
{
    public class GetFiltreStatusShipmentDto
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

        public bool IsActive { get; set; }

        public List<GetFiltreStatusShipmentTrackingDto> Trackings { get; set; }
    }

    public class GetFiltreStatusShipmentTrackingDto
    {
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string TrackingStatus { get; set; }
    }
}
