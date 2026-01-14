namespace TumorHospital.Application.DTOs.Request.Appointment
{
    public class PrescriptionCreateUpdateDto
    {
        public Guid AppointmentId { get; set; }
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
