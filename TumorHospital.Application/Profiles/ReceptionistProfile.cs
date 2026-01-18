using AutoMapper;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Profiles
{
    public class ReceptionistProfile : Profile
    {
        public ReceptionistProfile()
        {
            CreateMap<NewReceptionistDto, Receptionist>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ReverseMap();


        }
    }
}
