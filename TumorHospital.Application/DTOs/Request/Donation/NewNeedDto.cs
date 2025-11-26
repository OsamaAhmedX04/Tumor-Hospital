using Microsoft.AspNetCore.Http;

namespace TumorHospital.Application.DTOs.Request.Donation
{
    public class NewNeedDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string CharityCategory { get; set; }
        public decimal NeedAmount { get; set; }
    }
}
