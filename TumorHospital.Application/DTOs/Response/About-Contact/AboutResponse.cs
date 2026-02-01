namespace TumorHospital.Application.DTOs.Response.About_Contact
{
    public class AboutResponse
    {
        public string HospitalName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Mission { get; set; } = string.Empty;
        public string Vision { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalReceptionist { get; set; }
    }
}
