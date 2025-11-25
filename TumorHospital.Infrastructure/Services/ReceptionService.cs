using TumorHospital.Application.DTOs.Response.Bill;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.DTOs.Response.User;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class ReceptionService : IReceptionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReceptionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PageSourcePagination<BillDto>> GetAllBills(int pageSize, int pageNumber)
        {
            return await _unitOfWork.Bills.GetAllPaginatedEnhancedAsync(
                selector: b => new BillDto
                {
                    BillId = b.Id,
                    CreatedAt = b.CreatedAt,
                    PatientName = $"{b.Patient.User.FirstName} {b.Patient.User.FirstName}",
                    Status = b.Status,
                    TotalAmount = b.TotalAmount
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<BillDto>> GetBill(int pageSize, int pageNumber, string patientEmail)
        {
            return await _unitOfWork.Bills.GetAllPaginatedEnhancedAsync(
                filter: b => b.Patient.User.Email == patientEmail,
                selector: b => new BillDto
                {
                    BillId = b.Id,
                    CreatedAt = b.CreatedAt,
                    PatientName = $"{b.Patient.User.FirstName} {b.Patient.User.FirstName}",
                    Status = b.Status,
                    TotalAmount = b.TotalAmount
                },
                pageSize: pageSize,
                pageNumber: pageNumber
                );
        }

        public async Task ReceivePayment(Guid billId, string receptionistId, string billCode)
        {
            var bill = await _unitOfWork.Bills.GetByIdAsync(billId);

            if (bill == null)
                throw new Exception("This Bill Doesn't Exist");
            if (bill.Code != billCode)
                throw new Exception("Code Is Wrong");

            bill.PaymentDate = DateTime.Now;
            bill.ConfirmedBy = receptionistId;
            bill.Status = BillStatus.Paid;

            await _unitOfWork.CompleteAsync();

        }

        public async Task<PageSourcePagination<ReceptionistDto>> GetAllReceptionists(int pageSize, int PageNumber, string? receptionistName)
        {
            return await _unitOfWork.Receptionists.GetAllPaginatedEnhancedAsync(
                filter: r => ($"{r.User.FirstName} {r.User.LastName}").Contains(receptionistName ?? ""),
                selector: r => new ReceptionistDto
                {
                    Id = r.ApplicationUserId,
                    IsActive = r.User.IsActive,
                    Name = r.User.FirstName + " " + r.User.LastName,
                },
                pageSize: pageSize,
                pageNumber: PageNumber
                );
        }

        

        public async Task<ReceptionistDetailsDto> GetReceptionist(string id)
        {
            return await _unitOfWork.Receptionists.GetEnhancedAsync(
                filter: r => r.ApplicationUserId == id,
                selector: r => new ReceptionistDetailsDto
                {
                    Id = r.ApplicationUserId,
                    IsActive = r.User.IsActive,
                    IsDeleted = r.IsDeleted,
                    Name = r.User.FirstName + " " + r.User.LastName,
                    Email = r.User.Email!,
                    Gender = r.Gender
                }
                ) ?? new ReceptionistDetailsDto();
        }

        
    }
}
