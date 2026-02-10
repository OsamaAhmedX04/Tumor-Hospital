namespace TumorHospital.Application.Intefaces.Services
{
    public interface IVideoCallService
    {
        Task<Guid> StartVideoCallAsync(string patientId, string doctorId, Guid appointmentId);
        Task AcceptCallAsync(Guid callId, string userId);
        Task RejectCallAsync(Guid callId, string userId);
        Task EndCallAsync(Guid callId, string userId, string reason);
    }

}
