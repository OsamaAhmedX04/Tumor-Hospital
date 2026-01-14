namespace TumorHospital.Application.DTOs.Response.Appointment
{
    public class PrescriptionResponseDto
    {
        public Guid Id { get; set; }
        public Guid AppointmentId { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
