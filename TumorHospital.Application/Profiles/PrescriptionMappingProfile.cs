using AutoMapper;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Profiles
{
    public class PrescriptionMappingProfile : Profile
    {
        public PrescriptionMappingProfile()
        {
            CreateMap<Prescription, PrescriptionResponseDto>();
            CreateMap<PrescriptionCreateUpdateDto, Prescription>();
        }
    }
}
