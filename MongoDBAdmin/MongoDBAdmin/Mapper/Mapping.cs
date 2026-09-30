using AutoMapper;
using EntityLayer.Entities;
using MongoDBAdmin.Dto.About;
using MongoDBAdmin.Dto.Brand;
using MongoDBAdmin.Dto.GetInTouch;
using MongoDBAdmin.Dto.HowItWork;
using MongoDBAdmin.Dto.Offer;
using MongoDBAdmin.Dto.Role;
using MongoDBAdmin.Dto.Shipment;
using MongoDBAdmin.Dto.Slider;

namespace MongoDBAdmin.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<CreateSliderDto, Slider>().ReverseMap();
            CreateMap<UpdateSliderDto, Slider>().ReverseMap();

            CreateMap<CreateBrandDto, Brand>().ReverseMap();
            CreateMap<UpdateBrandDto, Brand>().ReverseMap();

            CreateMap<CreateOfferDto, Offer>().ReverseMap();
            CreateMap<UpdateOfferDto, Offer>().ReverseMap();

            CreateMap<CreateAboutDto, About>().ReverseMap();
            CreateMap<UpdateAboutDto, About>().ReverseMap();

            CreateMap<CreateGetInTouchDto, GetInTouch>().ReverseMap();
            CreateMap<UpdateGetInTouchDto, GetInTouch>().ReverseMap();
            CreateMap<CreateGetInTouchArticleDto, GetInTouchArticle>().ReverseMap();
            CreateMap<UpdateGetInTouchArticleDto, GetInTouchArticle>().ReverseMap();

            CreateMap<HowItWork, CreateHowItWorkDto>().ReverseMap();
            CreateMap<HowItWork, UpdateHowItWorkDto>().ReverseMap();
            CreateMap<HowItWorkArticle, CreateHowItWorkArticleDto>().ReverseMap();
            CreateMap<HowItWorkArticle, UpdateHowItWorkArticleDto>().ReverseMap();

            CreateMap<AppRole, RoleListDto>().ReverseMap();

            CreateMap<MongoDBAdmin.Dto.WhatWeHaveDone.CreateWhatWeHaveDoneDto, WhatWeHaveDone>().ReverseMap();
            CreateMap<MongoDBAdmin.Dto.WhatWeHaveDone.UpdateWhatWeHaveDoneDto, WhatWeHaveDone>().ReverseMap();

            CreateMap<MongoDBAdmin.Dto.FAQ.CreateFAQDto, FAQ>().ReverseMap();
            CreateMap<MongoDBAdmin.Dto.FAQ.UpdateFAQDto, FAQ>().ReverseMap();


            CreateMap<Shipment, CreateShipmentDto>().ReverseMap();
            CreateMap<ShipmentTracking, CreateShipmentTrackingDto>().ReverseMap();

            CreateMap<Shipment, UpdateShipmentDto>().ReverseMap();
            CreateMap<ShipmentTracking, UpdateShipmentTrackingDto>().ReverseMap();
        }

    }
}
