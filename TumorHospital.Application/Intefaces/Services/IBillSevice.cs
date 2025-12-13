using TumorHospital.Application.DTOs.Response.Bill;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IBillSevice
    {
        Task<PageSourcePagination<BillDto>> GetBills(
            int pageNumber, string? patientEmail = null, string? patientName = null, string? billCode = null
            );

        Task<PageSourcePagination<PatientBillDto>> GetPatientBills(int pageNumber, string patientId);

        Task ReceivePayment(Guid billId, string receptionistId, string billCode);
    }
}
