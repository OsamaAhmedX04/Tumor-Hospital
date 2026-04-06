namespace TumorHospital.Application.DTOs.Response.User
{
    public class DoctorDetailsDto
    {
        public string Id { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string Gender { get; set; } = null!;
        public string? Bio { get; set; }
        public string Specialization { get; set; }
        public bool IsVideoCallDoctor { get; set; }
        public decimal ConsultationCost { get; set; }
        public decimal FollowUpCost { get; set; }
        public decimal? VideoCallCost { get; set; }
        public string Location { get; set; } = null!;
        public bool IsAbleToAppointConsultation { get; set; }
        public bool IsAbleToAppointFollowUp { get; set; }
        public bool IsAbleToAppointVideoCall { get; set; }
        public List<DoctorWorkDayDto> WorkingDays { get; set; } = new List<DoctorWorkDayDto>();
    }
}
