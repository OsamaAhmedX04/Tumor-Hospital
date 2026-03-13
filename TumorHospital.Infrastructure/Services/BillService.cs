using Microsoft.Extensions.Logging;
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
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<BillService> _logger;
        public BillService(IUnitOfWork unitOfWork, IAppointmentService appointmentService, ILogger<BillService> logger)
        {
            _unitOfWork = unitOfWork;
            _appointmentService = appointmentService;
            _logger = logger;
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
                    AppointmentDate = b.Appointment!.AttendenceDate!.Value,
                    PatientName = $"{b.Patient.User.FirstName} {b.Patient.User.LastName}",
                    Status = b.Status.ToString(),
                    TotalAmount = b.FinalAmount
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
                    AppointmentDate = b.Appointment!.AttendenceDate!.Value,
                    Status = b.Status.ToString(),
                    TotalAmount = b.FinalAmount,
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
            {
                _logger.LogWarning(
                    "Payment failed: Bill not found. BillId={BillId}",
                    billId
                );
                throw new Exception("This Bill Doesn't Exist");
            }

            if (bill.Code != billCode)
            {
                _logger.LogWarning(
                    "Payment failed: Invalid bill code. BillId={BillId}",
                    billId
                );
                throw new Exception("Code Is Wrong");
            }

            bill.PaymentDate = DateTime.Now;
            bill.ConfirmedBy = receptionistId;
            bill.Status = BillStatus.Paid;

            await _appointmentService.AttendPatientToAppointment(bill.PatientId, bill.AppointmentId!.Value);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation(
                "Paying bill for PatientId {PatientId}, AppointmentId {AppointmentId}",
                bill.PatientId,
                bill.AppointmentId
            );
        }
    }
}
