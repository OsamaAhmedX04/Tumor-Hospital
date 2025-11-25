namespace TumorHospital.Application.DTOs.Request.Donation
{
    public class UpdateNeedDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string CharityCategory { get; set; }
        public decimal NeedAmount { get; set; }
    }
}
