using AutoMapper;
using TumorHospital.Domain.Entities;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Domain.Constants;

namespace TumorHospital.Application.Profiles
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {

            CreateMap<Patient, PatientProfileResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<UpdatePatientProfileDto, Patient>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdatePatientProfileDto, ApplicationUser>();

            CreateMap<Doctor, DoctorProfileResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => SupabaseConstants.PrefixSupaURL + src.ProfilePicturePath))
                .ForMember(dest => dest.SpecializationName, opt => opt.MapFrom(src => src.Specialization.Name));

            CreateMap<UpdateDoctorProfileDto, Doctor>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateDoctorProfileDto, ApplicationUser>();

            CreateMap<Receptionist, ReceptionistProfileResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email));

            CreateMap<UpdateReceptionistProfileDto, Receptionist>()
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<UpdateReceptionistProfileDto, ApplicationUser>();
        }
    }

}
