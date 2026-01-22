using TumorHospital.Application.DTOs.Response.Bill;
using TumorHospital.Application.DTOs.Response.Pagination;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.Services
{
    public class BillService : IBillSevice
    {

        private readonly IUnitOfWork _unitOfWork;
        public BillService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageSourcePagination<BillDto>> GetBills(
            int pageNumber, string? patientEmail = null, string? patientName = null, string? billCode = null)
        {
            return await _unitOfWork.Bills.GetAllPaginatedEnhancedAsync(
                filter: b => b.Patient.User.Email == patientEmail
                ||
                (b.Patient.User.FirstName + " " + b.Patient.User.LastName).Contains(patientName ?? "")
                ||
                b.Code == billCode,
                selector: b => new BillDto
                {
                    BillId = b.Id,
                    CreatedAt = b.CreatedAt,
                    PatientName = $"{b.Patient.User.FirstName} {b.Patient.User.LastName}",
                    Status = b.Status,
                    TotalAmount = b.TotalAmount
                },
                pageSize: 20,
                pageNumber: pageNumber
                );
        }

        public async Task<PageSourcePagination<PatientBillDto>> GetPatientBills(int pageNumber, string patientId)
        {
            if (!await _unitOfWork.Patients.IsExistAsync(patientId))
                throw new Exception("Patient Not Found");

            var bills = await _unitOfWork.Bills.GetAllPaginatedEnhancedAsync(
                filter: b => b.PatientId == patientId,
                selector: b => new PatientBillDto
                {
                    BillId = b.Id,
                    CreatedAt = b.CreatedAt,
                    PatientName = $"{b.Patient.User.FirstName} {b.Patient.User.LastName}",
                    Status = b.Status,
                    TotalAmount = b.TotalAmount,
                    BillCode = b.Code
                },
                pageNumber: pageNumber,
                pageSize: 20
                );

            return bills;
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
    }
}
