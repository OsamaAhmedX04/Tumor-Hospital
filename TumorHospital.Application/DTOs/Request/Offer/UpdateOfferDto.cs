namespace TumorHospital.Application.DTOs.Request.Offer
{
    public class UpdateOfferDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
