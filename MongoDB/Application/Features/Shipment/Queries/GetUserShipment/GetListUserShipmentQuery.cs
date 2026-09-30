using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using Application.Features.Shipment.Queries.GetList;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Shipment.Queries.GetUserShipment
{
    public class GetListUserShipmentQuery:IRequest<Paginate<GetListUserShipmentDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
        public Guid AppUserId { get; set; }
        public string? Status { get; set; }
    }

    public class GetListUserShipmentQueryHandler : IRequestHandler<GetListUserShipmentQuery, Paginate<GetListUserShipmentDto>>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;

        public GetListUserShipmentQueryHandler(IShipmentRepository shipmentRepository, IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListUserShipmentDto>> Handle(GetListUserShipmentQuery request, CancellationToken cancellationToken)
        {
            var shipments = await _shipmentRepository.GetListAsync(
                size: request.pageResult.Size,
                page: request.pageResult.Index,
                predicate: N => N.ReceiverUserId == request.AppUserId && (string.IsNullOrEmpty(request.Status) || N.CurrentStatus == request.Status)
            );

            var mappedShipments = _mapper.Map<Paginate<GetListUserShipmentDto>>(shipments);
            return mappedShipments;
        }
    }
}
