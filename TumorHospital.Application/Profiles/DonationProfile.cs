using AutoMapper;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Response.Donation;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Profiles
{
    public class DonationProfile : Profile
    {
        public DonationProfile()
        {
            CreateMap<NeedDetailsDto, CharityNeed>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CharityCategory>(src.CharityCategory, true)))
                .ReverseMap()
                .ForMember(dest => dest.CharityCategory, opt => opt.MapFrom(src => src.Category.ToString()));

            CreateMap<NewNeedDto, CharityNeed>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CharityCategory>(src.CharityCategory, true)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CollectedAmount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => false));

            CreateMap<VolunteerInfoDto, VolunteerDonation>();



        }
    }
}
