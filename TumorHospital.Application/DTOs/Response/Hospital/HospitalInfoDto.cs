namespace TumorHospital.Application.DTOs.Response.Hospital
{
    public class HospitalInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Government { get; set; }
        public string Address { get; set; }
        public int MaxNumberOfDoctors { get; set; }
        public int NumberOfDoctors { get; set; }
        public int MaxNumberOfReceptionists { get; set; }
        public int NumberOfReceptionists { get; set; }
    }
}
