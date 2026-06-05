namespace TumorHospital.Application.DTOs.Request.Appointment
{
    public class PrescriptionUpdateDto
    {
        public string Medication { get; set; }
        public string Dosage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
