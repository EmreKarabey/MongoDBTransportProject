using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Entities
{
    public class ShipmentTracking
    {
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string TrackingStatus { get; set; }
    }
}
