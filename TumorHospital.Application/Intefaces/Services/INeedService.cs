using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Application.DTOs.Response.Donation;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface INeedService
    {
        Task<PageSourcePagination<NeedDto>> GetAllNeeds(int pageNumber);
        Task<NeedDetailsDto> GetNeed(Guid id);
        CharityCategoriesDto GetCategoriesOfNeeds();
        Task AddNeed(NewNeedDto need);
        Task UpdateNeed(UpdateNeedDto newNeed, Guid id);
        Task DeleteNeed(Guid id);
    }
}
