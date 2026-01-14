using TumorHospital.Domain.Entities;

namespace TumorHospital.Application.Helpers
{
    public class AppointmentTimeService
    {
        public bool HasAppointmentHappened(Appointment appointment)
        {
            if (appointment == null)
                return false;

            if (appointment.AttendenceDate == null || appointment.FromTime == null)
                return false;

            var timeOnly = TimeOnly.FromTimeSpan(appointment.FromTime.Value);

            var appointmentDateTime =
                appointment.AttendenceDate.Value.ToDateTime(timeOnly);

            return appointmentDateTime <= DateTime.Now;
        }
    }
}
