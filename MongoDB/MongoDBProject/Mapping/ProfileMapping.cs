using AutoMapper;
using Domain.Entities;
using MongoDBProject.Models;

namespace MongoDBProject.Mapping
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<AppUser, ProfileViewModel>()
                .ForMember(dest => dest.ExistingImageUrl, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.RemoveImage, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.TotalShipments, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentPassword, opt => opt.Ignore())
                .ForMember(dest => dest.NewPassword, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmNewPassword, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.TotalShipments, opt => opt.Ignore());

            // ProfileViewModel -> AppUser (Güncelleme için, sadece gerekli alanlar)
            CreateMap<ProfileViewModel, AppUser>(MemberList.None)
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
