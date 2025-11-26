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
        public decimal CollectedAmount { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
