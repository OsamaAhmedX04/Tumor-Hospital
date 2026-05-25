using TumorHospital.Domain.Constants;
using TumorHospital.Domain.Entities;

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

        public static TimeSlot? SelectValidTimeSlot
            (HashSet<TimeSpan> appointmentedTimesForPatientInRequestedDay, HashSet<TimeSpan> appointmentedTimesForDoctorInRequestedDay,
            TimeSpan doctorStartTime, TimeSpan doctorEndTime)
        {

            var timeSlot = doctorStartTime;

            for (int i = 0; i < Appointments.NumberOfAppointmentsPerDay; i++)
            {
                bool isBusyTimeSlotForPatient = appointmentedTimesForPatientInRequestedDay.Contains(timeSlot);
                bool isBusyTimeSlotForDoctor = appointmentedTimesForDoctorInRequestedDay.Contains(timeSlot);

                if (!isBusyTimeSlotForPatient && !isBusyTimeSlotForDoctor)
                {
                    if (timeSlot.Add(TimeSpan.FromMinutes(30)) > doctorEndTime) break;

                    return new TimeSlot { FromTime = timeSlot, ToTime = timeSlot.Add(TimeSpan.FromMinutes(30)) };
                }

                timeSlot = timeSlot.Add(TimeSpan.FromMinutes(30));
            }

            return null;
        }

        public static bool IsInValidTimeToAppoint()
            => DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 5;
    }
}
