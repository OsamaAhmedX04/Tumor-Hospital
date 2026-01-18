namespace TumorHospital.Application.DTOs.Request.Hospital
{
    public class HospitalDto
    {
        public string Name { get; set; }
        public string Government { get; set; }
        public string Address { get; set; }
        public int MaxNumberOfDoctors { get; set; }
        public int MaxNumberOfReceptionists { get; set; }
    }
}
