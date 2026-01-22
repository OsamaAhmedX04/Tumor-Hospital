using Microsoft.EntityFrameworkCore;
using TumorHospital.Application.Intefaces.BackgroundServices;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Infrastructure.BackgroundJobs.BackgroundServices
{
    public class BackgroundAppointment : IBackgroundAppointment
    {
        private readonly IUnitOfWork _unitOfWork;

        public BackgroundAppointment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task SetApprovedAppointmentsStatusToAbsent()
        {
            await _unitOfWork.Appointments
                .GetAllAsIQueryable()
                .Where(a => a.Status == AppointmentStatus.Approved)
                .ExecuteUpdateAsync(setter => setter.SetProperty(a => a.Status, AppointmentStatus.Absent));
        }
    }
}
