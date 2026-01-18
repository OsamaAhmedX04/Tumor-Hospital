using AutoMapper;
using TumorHospital.Application.DTOs.Request.FAQs;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Profiles
{
    public class FAQProfile : Profile
    {
        public FAQProfile()
        {
            CreateMap<NewFAQsDto, FAQ>().ReverseMap();
        }
    }
}
