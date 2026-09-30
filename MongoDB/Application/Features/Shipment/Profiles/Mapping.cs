
using Application.Features.Shipment.Queries.GetById;
using Application.Features.Shipment.Queries.GetList;
using Application.PageResult;
using AutoMapper;
using Domain.Entities;
using Persistence.Paginate;

namespace Application.Features.Shipment.Profiles
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Domain.Entities.Shipment, GetListShipmentDto>().ReverseMap();
            CreateMap<Domain.Entities.Shipment, GetByIdShipmentDto>().ReverseMap();
            CreateMap<Domain.Entities.Shipment, Application.Features.Shipment.Queries.GetUserShipment.GetListUserShipmentDto>().ReverseMap();
            CreateMap<Domain.Entities.Shipment, Application.Features.Shipment.Queries.GetFiltreStatus.GetFiltreStatusShipmentDto>().ReverseMap();

            CreateMap<Paginate<Domain.Entities.Shipment>, Paginate<GetListShipmentDto>>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Shipment>, Paginate<Application.Features.Shipment.Queries.GetUserShipment.GetListUserShipmentDto>>().ReverseMap();
            CreateMap<Paginate<Domain.Entities.Shipment>, Paginate<Application.Features.Shipment.Queries.GetFiltreStatus.GetFiltreStatusShipmentDto>>().ReverseMap();
        }
    }
}
