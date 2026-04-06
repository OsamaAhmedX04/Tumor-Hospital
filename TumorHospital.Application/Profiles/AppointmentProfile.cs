using AutoMapper;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Profiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<NewConsultationAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Pending))
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => Enum.Parse<Day>(src.DayOfWeek, true)))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => AppointmentReason.Consultation))
                .ForMember(dest => dest.RequestCreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<NewFollowUpAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Pending))
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => Enum.Parse<Day>(src.DayOfWeek, true)))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => AppointmentReason.FollowUp))
                .ForMember(dest => dest.RequestCreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<NewVideoCallAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppointmentStatus.Pending))
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => Enum.Parse<Day>(src.DayOfWeek, true)))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => AppointmentReason.VideoCall))
                .ForMember(dest => dest.RequestCreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}
