using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
