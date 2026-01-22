namespace TumorHospital.Application.DTOs.Response.Admin
{
    public class AdminDashboardResponse
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalReceptionists { get; set; }
        public int TotalAppointments { get; set; }
        public int PendingBills { get; set; }
        public int TotalBills { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCharityNeeds { get; set; }
        public int CompletedCharityNeeds { get; set; }
    }
}
