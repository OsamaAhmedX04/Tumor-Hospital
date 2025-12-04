using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAppointmentService
    {
        Task AppointConsultation(NewConsultationAppointmentDto appointment);
        Task AppointFollowUp(NewFollowUpAppointmentDto appointment);
        Task AppointSurgery(NewSurgeryAppointmentDto appointment);

        Task<PageSourcePagination<AppointmentDto>> GetAppointments(int pageNumber, string? appointmentReason = null, string? appointmentStatus = null);
        Task<PageSourcePagination<AppointmentDto>> GetPatientAppointments(int pageNumber, string patientId, string? appointmentReason = null, string? appointmentStatus = null);
        List<string> AppointmentReasons();
        Task AcceptAppointment(Guid appointmentId, AppointmentSetterDateTimeDto setter);
        Task RejectAppointment(Guid appointmentId);
    }
}
