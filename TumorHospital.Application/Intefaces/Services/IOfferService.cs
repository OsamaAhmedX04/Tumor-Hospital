using TumorHospital.Application.DTOs.Request.Offer;
using TumorHospital.Application.DTOs.Response.Offer;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IOfferService
    {
        Task<OfferResponse> AddOfferAsync(AddOfferDto dto);
        Task<OfferResponse> UpdateOfferAsync(Guid id, UpdateOfferDto dto);
        Task<bool> RemoveOfferAsync(Guid id);
        Task<List<OfferResponse>> GetAllOffersAsync();
        Task<List<OfferResponse>> GetExpiredOffersAsync();
        Task<List<OfferResponse>> GetUpcomingOffersAsync();
        Task ActivateOfferAsync(Guid id);
        Task DeactivateOfferAsync(Guid id);
    }
}
