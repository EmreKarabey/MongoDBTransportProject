using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Shipment.Queries.GetList;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Shipment.Queries.GetFiltreStatus
{
    public class GetFiltreStatusShipmentQuery : IRequest<Paginate<GetFiltreStatusShipmentDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
        public string? Filtre { get; set; }
    }

    public class GetListShipmentQueryHandler : IRequestHandler<GetFiltreStatusShipmentQuery, Paginate<GetFiltreStatusShipmentDto>>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;

        public GetListShipmentQueryHandler(IShipmentRepository shipmentRepository, IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetFiltreStatusShipmentDto>> Handle(GetFiltreStatusShipmentQuery request, CancellationToken cancellationToken)
        {
            var shipments = await _shipmentRepository.GetListAsync(
                size: request.pageResult.Size,
                page: request.pageResult.Index,
                predicate:n=>n.CurrentStatus==request.Filtre
            );

            var mappedShipments = _mapper.Map<Paginate<GetFiltreStatusShipmentDto>>(shipments);
            return mappedShipments;
        }
    }
}
