namespace TumorHospital.Application.DTOs.Request.Offer
{
    public class AddOfferDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}