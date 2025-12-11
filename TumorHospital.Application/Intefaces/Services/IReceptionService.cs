using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Response.Bill;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IReceptionService
    {
        Task<PageSourcePagination<ReceptionistDto>> GetAllReceptionists(int PageNumber, string? receptionistName);

        Task<ReceptionistDetailsDto> GetReceptionist(string id);

        //Task<PageSourcePagination<BillDto>> GetAllBills(int pageNumber);

        Task<PageSourcePagination<BillDto>> GetBills(
            int pageNumber, string? patientEmail = null, string? patientName = null, string? billCode = null
            );

        Task ReceivePayment(Guid billId, string receptionistId, string billCode);
    }
}
