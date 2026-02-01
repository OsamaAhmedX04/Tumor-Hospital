using AutoMapper;
using TumorHospital.Domain.Entities;
using TumorHospital.Application.DTOs.Request.Offer;
using TumorHospital.Application.DTOs.Response.Offer;

namespace TumorHospital.Application.Profiles
{
    public class OfferMappingProfile : Profile
    {
        public OfferMappingProfile()
        {
            CreateMap<AddOfferDto, Offer>();
            CreateMap<UpdateOfferDto, Offer>();
            CreateMap<Offer, OfferResponse>();
        }
    }
}