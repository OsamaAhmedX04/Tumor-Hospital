using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Caching.Memory;
using TumorHospital.Application.DTOs.Request.Offer;
using TumorHospital.Application.DTOs.Response.Offer;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class OfferService : IOfferService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public OfferService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<OfferResponse> AddOfferAsync(AddOfferDto dto)
        {
            var offer = _mapper.Map<Offer>(dto);
            offer.IsActive = false;
            await _unitOfWork.Offers.AddAsync(offer);
            await _unitOfWork.CompleteAsync();

            var now = DateTime.Now;

            if (offer.StartDate > now)
                BackgroundJob.Schedule(() => ActivateOfferAsync(offer.Id), offer.StartDate - now);
            else
                await ActivateOfferAsync(offer.Id);

            if (offer.EndDate > now)
                BackgroundJob.Schedule(() => DeactivateOfferAsync(offer.Id), offer.EndDate - now);

            _cache.Remove("active_offers");
            _cache.Remove("upcoming_offers");

            return _mapper.Map<OfferResponse>(offer);
        }

        public async Task<OfferResponse> UpdateOfferAsync(Guid id, UpdateOfferDto dto)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(id);
            if (offer == null) throw new Exception("Offer not found");

            _mapper.Map(dto, offer);
            _unitOfWork.Offers.Update(offer);
            await _unitOfWork.CompleteAsync();

            var now = DateTime.Now;

            if (offer.StartDate > now)
                BackgroundJob.Schedule(() => ActivateOfferAsync(offer.Id), offer.StartDate - now);
            else if (!offer.IsActive && offer.StartDate <= now && offer.EndDate > now)
                await ActivateOfferAsync(offer.Id);

            if (offer.EndDate > now)
                BackgroundJob.Schedule(() => DeactivateOfferAsync(offer.Id), offer.EndDate - now);
            else if (offer.EndDate <= now)
                await DeactivateOfferAsync(offer.Id);

            _cache.Remove("active_offers");
            _cache.Remove("upcoming_offers");

            return _mapper.Map<OfferResponse>(offer);
        }

        public async Task<bool> RemoveOfferAsync(Guid id)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(id);
            if (offer == null) return false;

            _unitOfWork.Offers.Delete(id);
            await _unitOfWork.CompleteAsync();

            _cache.Remove("active_offers");
            _cache.Remove("upcoming_offers");

            return true;
        }

        public async Task<List<OfferResponse>> GetAllOffersAsync()
        {
            const string cacheKey = "active_offers";

            if (_cache.TryGetValue(cacheKey, out List<OfferResponse> cached))
                return cached;

            var now = DateTime.Now;

            var offers = await _unitOfWork.Offers.GetAllAsync(
                o => o,
                o => o.IsActive && o.StartDate <= now && o.EndDate >= now
            );

            var result = offers.Select(o => _mapper.Map<OfferResponse>(o)).ToList();

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(10)
            });

            return result;
        }

        public async Task<List<OfferResponse>> GetExpiredOffersAsync()
        {
            var now = DateTime.Now;

            var expiredOffers = await _unitOfWork.Offers.GetAllAsync(
                o => o,
                o => o.EndDate < now
            );

            return expiredOffers.Select(o => _mapper.Map<OfferResponse>(o)).ToList();
        }

        public async Task<List<OfferResponse>> GetUpcomingOffersAsync()
        {
            const string cacheKey = "upcoming_offers";

            if (_cache.TryGetValue(cacheKey, out List<OfferResponse> cached))
                return cached;

            var now = DateTime.Now;
            var offers = await _unitOfWork.Offers.GetAllAsync(
                o => o,
                o => o.StartDate > now
            );

            var result = offers.Select(o => _mapper.Map<OfferResponse>(o)).ToList();

            _cache.Set(cacheKey, result, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(10)
            });

            return result;
        }

        public async Task ActivateOfferAsync(Guid id)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(id);
            if (offer == null) return;

            var now = DateTime.Now;

            if (now >= offer.StartDate && now < offer.EndDate)
            {
                offer.IsActive = true;
                _unitOfWork.Offers.Update(offer);
                await _unitOfWork.CompleteAsync();
            }
        }

        public async Task DeactivateOfferAsync(Guid id)
        {
            var offer = await _unitOfWork.Offers.GetByIdAsync(id);
            if (offer == null) return;

            offer.IsActive = false;
            _unitOfWork.Offers.Update(offer);
            await _unitOfWork.CompleteAsync();
        }
    }
}
