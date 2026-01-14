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
        private readonly AppointmentTimeService _appointmentTimeService;

        public PrescriptionService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            AppointmentTimeService appointmentTimeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appointmentTimeService = appointmentTimeService;
        }

        public async Task<PrescriptionResponseDto> CreateAsync(PrescriptionCreateUpdateDto dto)
        {
            var appointment = await _unitOfWork.Appointments.GetAsync(
                x => x,
                x => x.Id == dto.AppointmentId,
                Includes: x => x.Prescription
            );

            if (appointment == null)
                throw new Exception("Appointment not found");

            if (!_appointmentTimeService.HasAppointmentHappened(appointment))
                throw new Exception("Cannot create prescription before appointment happens");

            if (appointment.Prescription != null)
                throw new Exception("Prescription already exists");

            var prescription = _mapper.Map<Prescription>(dto);

            await _unitOfWork.Prescriptions.AddAsync(prescription);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PrescriptionResponseDto>(prescription);
        }

        public async Task<PrescriptionResponseDto> GetByAppointmentIdAsync(Guid appointmentId)
        {
            var prescription = await _unitOfWork.Prescriptions.GetAsync(
                x => x,
                x => x.AppointmentId == appointmentId
            );

            if (prescription == null)
                throw new Exception("Prescription not found");

            return _mapper.Map<PrescriptionResponseDto>(prescription);
        }

        public async Task<bool> UpdateAsync(Guid id, PrescriptionCreateUpdateDto dto)
        {
            var prescription = await _unitOfWork.Prescriptions.GetAsync(
                x => x,
                x => x.Id == id,
                Includes: x => x.Appointment
            );

            if (prescription == null)
                return false;

            if (!_appointmentTimeService.HasAppointmentHappened(prescription.Appointment))
                throw new Exception("Cannot update prescription before appointment happens");

            _mapper.Map(dto, prescription);

            _unitOfWork.Prescriptions.Update(prescription);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var prescription = await _unitOfWork.Prescriptions.GetAsync(
                x => x,
                x => x.Id == id,
                Includes: x => x.Appointment
            );

            if (prescription == null)
                return false;

            if (!_appointmentTimeService.HasAppointmentHappened(prescription.Appointment))
                throw new Exception("Cannot delete prescription before appointment happens");

            _unitOfWork.Prescriptions.Delete(id);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }

}
