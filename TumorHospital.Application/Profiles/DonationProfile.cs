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
            CreateMap<NeedDetailsDto,CharityNeed>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src =>  Enum.Parse<CharityCategory>(src.CharityCategory, true)))
                .ReverseMap()
                .ForMember(dest => dest.CharityCategory,opt => opt.MapFrom(src => src.Category.ToString()));

            CreateMap<NewNeedDto, CharityNeed>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => Enum.Parse<CharityCategory>(src.CharityCategory, true)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<VolunteerInfoDto, VolunteerDonation>();



        }
    }
}
