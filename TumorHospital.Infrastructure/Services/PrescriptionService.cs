using AutoMapper;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Application.Helpers;
using TumorHospital.Application.Intefaces.Services;
using TumorHospital.Application.Intefaces.UOW;
using TumorHospital.Domain.Entities;

namespace TumorHospital.Infrastructure.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PrescriptionService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PrescriptionResponseDto> GetByAppointmentIdAsync(Guid appointmentId)
        {
            var prescription = await _unitOfWork.Prescriptions.GetAsync(
                x => new PrescriptionResponseDto
                {
                    PrescriptionId = x.Id,
                    AppointmentId = x.AppointmentId,
                    Medication = x.Medication,
                    Dosage = x.Dosage,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate
                },
                x => x.AppointmentId == appointmentId
            );

            if (prescription == null)
                throw new Exception("Prescription not found");

            return prescription;
        }

        public async Task CreateAsync(Guid appointmentId, PrescriptionCreateDto dto)
        {
            var appointment = await _unitOfWork.Appointments.GetAsync(
                x => x,
                x => x.Id == appointmentId,
                Includes: x => x.Prescription
            );

            if (appointment == null)
                throw new Exception("Appointment not found");
            var egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");

            var egyptNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptTimeZone);
            var appointmentEnd =
                appointment.AttendenceDate.Value.ToDateTime(
                    TimeOnly.FromTimeSpan(appointment.ToTime.Value)
                );

            if (egyptNow < appointmentEnd)
                throw new Exception($"Cannot create prescription before appointment ends{appointmentEnd.ToString()} | {egyptNow.ToString()}");
            //if (!AppointmentTimeService.HasAppointmentHappened(appointment))
            //    throw new Exception("Cannot create prescription before appointment happens");

            if (appointment.Prescription != null)
                throw new Exception("Prescription already exists");

            var prescription = _mapper.Map<Prescription>(dto);
            prescription.AppointmentId = appointmentId;


            await _unitOfWork.Prescriptions.AddAsync(prescription);
            await _unitOfWork.CompleteAsync();
        }



        public async Task UpdateAsync(Guid id, PrescriptionUpdateDto dto)
        {
            var prescription = await _unitOfWork.Prescriptions.GetAsync(
                x => x,
                x => x.Id == id,
                Includes: x => x.Appointment
            );

            if (prescription == null)
                throw new Exception("Prescription not found");

            if (!AppointmentTimeService.HasAppointmentHappened(prescription.Appointment))
                throw new Exception("Cannot update prescription before appointment happens");

            _mapper.Map(dto, prescription);

            _unitOfWork.Prescriptions.Update(prescription);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var prescription = await _unitOfWork.Prescriptions.GetAsync(
                x => x,
                x => x.Id == id,
                Includes: x => x.Appointment
            );

            if (prescription == null)
                throw new Exception("Prescription not found");

            if (!AppointmentTimeService.HasAppointmentHappened(prescription.Appointment))
                throw new Exception("Cannot delete prescription before appointment happens");

            _unitOfWork.Prescriptions.Delete(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
