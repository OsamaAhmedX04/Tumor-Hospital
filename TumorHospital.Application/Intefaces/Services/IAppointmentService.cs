using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Application.DTOs.Response.Pagination;

namespace TumorHospital.Application.Intefaces.Services
{
    public interface IAppointmentService
    {
        Task AppointConsultation(NewConsultationAppointmentDto appointment);
        Task AppointFollowUp(NewConsultationAppointmentDto appointment);
        Task AppointSurgery(NewConsultationAppointmentDto appointment);

        Task<PageSourcePagination<AppointmentDto>> GetAppointments(int pageNumber);

        Task AcceptAppointment(Guid appointmentId, AppointmentSetterDateTimeDto setter);
        Task RejectAppointment(Guid appointmentId);
    }
}
