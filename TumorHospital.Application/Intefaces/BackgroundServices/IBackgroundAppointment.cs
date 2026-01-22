namespace TumorHospital.Application.Intefaces.BackgroundServices
{
    public interface IBackgroundAppointment
    {
        Task SetApprovedAppointmentsStatusToAbsent();
    }
}
