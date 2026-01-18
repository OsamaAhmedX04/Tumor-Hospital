using AutoMapper;
using TumorHospital.Application.DTOs.Request.Hospital;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Profiles
{
    public class HospitalProfile : Profile
    {
        public HospitalProfile()
        {
            CreateMap<HospitalDto, Hospital>();
        }
    }
}
