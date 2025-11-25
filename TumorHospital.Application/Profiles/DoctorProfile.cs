using AutoMapper;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Profiles
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<NewDoctorDto, Doctor>()
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(src =>DateTime.Now))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src =>src.Schedules))
                .ReverseMap();

            CreateMap<DoctorScheduleDto, DoctorSchedule>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => Enum.Parse<Day>(src.DayOfWeek, true)))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.StartTime.Add(TimeSpan.FromHours(8))))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => true))
                .ReverseMap();
        }
    }
}
