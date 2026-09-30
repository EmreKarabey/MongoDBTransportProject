using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.PageResult;
using Application.Services.Repository;
using AutoMapper;
using MediatR;
using Persistence.Paginate;

namespace Application.Features.Shipment.Queries.GetList
{
    public class GetListShipmentQuery : IRequest<Paginate<GetListShipmentDto>>
    {
        public Application.PageResult.PageResult pageResult { get; set; }
    }

    public class GetListShipmentQueryHandler : IRequestHandler<GetListShipmentQuery, Paginate<GetListShipmentDto>>
    {
        private readonly IShipmentRepository _shipmentRepository;
        private readonly IMapper _mapper;

        public GetListShipmentQueryHandler(IShipmentRepository shipmentRepository, IMapper mapper)
        {
            _shipmentRepository = shipmentRepository;
            _mapper = mapper;
        }

        public async Task<Paginate<GetListShipmentDto>> Handle(GetListShipmentQuery request, CancellationToken cancellationToken)
        {
            var shipments = await _shipmentRepository.GetListAsync(
                size: request.pageResult.Size,
                page: request.pageResult.Index
            );

            var mappedShipments = _mapper.Map<Paginate<GetListShipmentDto>>(shipments);
            return mappedShipments;
        }
    }
}
