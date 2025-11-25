using Microsoft.AspNetCore.Mvc;
using TumorHospital.Application.DTOs.Response.Bill;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IReceptionService
    {
        Task<PageSourcePagination<ReceptionistDto>> GetAllReceptionists(int pageSize, int PageNumber, string? receptionistName);

        Task<ReceptionistDetailsDto> GetReceptionist(string id);

        Task<PageSourcePagination<BillDto>> GetAllBills(int pageSize, int pageNumber);

        Task<PageSourcePagination<BillDto>> GetBill(int pageSize, int pageNumber, string patientEmail);

        Task ReceivePayment(Guid billId, string receptionistId, string billCode);
    }
}
