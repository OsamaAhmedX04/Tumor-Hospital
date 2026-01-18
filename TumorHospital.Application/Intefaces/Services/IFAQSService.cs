using TumorHospital.Application.DTOs.Request.FAQs;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IFAQSService
    {
        Task<List<FAQsDto>> GetAllFAQs();
        Task AddFAQ(NewFAQsDto dto);
        Task UpdateFAQ(int id, NewFAQsDto dto);
        Task DeleteFAQ(int id);
    }
}
