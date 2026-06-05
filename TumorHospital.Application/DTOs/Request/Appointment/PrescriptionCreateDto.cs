namespace TumorHospital.Application.DTOs.Request.Appointment
{
    public class PrescriptionCreateDto
    {
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
