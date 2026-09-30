using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Shipment.Rules;
using Application.Services.Repository;
using AutoMapper;
using MediatR;

namespace Application.Features.Shipment.Queries.GetById
{
    public class GetByIdShipmentQuery : IRequest<GetByIdShipmentDto>
    {
        public string ShipmentId { get; set; }

        public GetByIdShipmentQuery(string shipmentId)
        {
            ShipmentId = shipmentId;
        }
    }

    public class GetByIdShipmentQueryHandler : IRequestHandler<GetByIdShipmentQuery, GetByIdShipmentDto>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;
        private readonly ShipmentBusinessRules _shipmentBusinessRules;

        public GetByIdShipmentQueryHandler(IShipmentRepository shipmentRepository, IMapper mapper, ShipmentBusinessRules shipmentBusinessRules)
        {
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
            _shipmentBusinessRules = shipmentBusinessRules;
        }

        public async Task<GetByIdShipmentDto> Handle(GetByIdShipmentQuery request, CancellationToken cancellationToken)
        {
            var shipment = await _shipmentRepository.GetByIdAsync(request.ShipmentId);
            _shipmentBusinessRules.CheckIfShipmentExists(shipment);

            var dto = _mapper.Map<GetByIdShipmentDto>(shipment);
            return dto;
        }
    }
}
