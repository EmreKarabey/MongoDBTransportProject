using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Shipment.Constants;
using Application.Services.Repository;

namespace Application.Features.Shipment.Rules
{
    public class ShipmentBusinessRules
    {
        private readonly IShipmentRepository _shipmentRepository;

        public ShipmentBusinessRules(IShipmentRepository shipmentRepository)
        {
            _shipmentRepository = shipmentRepository;
        }

        public void CheckIfShipmentExists(Domain.Entities.Shipment? shipment)
        {
            if (shipment == null)
            {
                throw new Exception(ShipmentMessages.NotFound);
            }
        }
    }
}
