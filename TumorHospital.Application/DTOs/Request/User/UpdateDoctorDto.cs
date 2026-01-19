namespace TumorHospital.Application.DTOs.Request.User
{
    public class UpdateDoctorDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string SpecializationName { get; set; }
        public bool IsSurgeon { get; set; }
        public decimal ConsultationCost { get; set; }
        public decimal FollowUpCost { get; set; }
        public decimal? SurgeryCost { get; set; }

    }
}
