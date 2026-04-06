using TumorHospital.Application.DTOs.Response.Meeting;

namespace TumorHospital.Application.Intefaces.ExternalServices
{
    public interface IMeetingService
    {
        Task<CreateMeetingDto> CreateMeeting();
    }
}
