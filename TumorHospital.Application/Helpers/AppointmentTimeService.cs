using TumorHospital.Application.DTOs.Response.Appointment;
using TumorHospital.Domain.Entities;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Helpers
{
    public static class AppointmentTimeService
    {
        public static bool HasAppointmentHappened(Appointment appointment)
        {
            if (appointment == null)
                return false;

            if (appointment.AttendenceDate == null || appointment.FromTime == null)
                return false;

            var today = DateOnly.FromDateTime(DateTime.Now);
            var appointmentDate = appointment.AttendenceDate.Value;

            if (appointmentDate > today)
                return false;

            if (appointmentDate < today)
                return true;

            var appointmentTime = TimeOnly.FromTimeSpan(appointment.FromTime.Value);
            var nowTime = TimeOnly.FromDateTime(DateTime.Now);

            return nowTime >= appointmentTime;
        }
    }
}
