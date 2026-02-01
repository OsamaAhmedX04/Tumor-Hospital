using FluentValidation;
using TumorHospital.Application.DTOs.Request.Offer;

namespace TumorHospital.Application.Validators.Offer
{
    public class UpdateOfferDtoValidator : AbstractValidator<UpdateOfferDto>
    {
        public UpdateOfferDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().MaximumLength(100);

            RuleFor(x => x.DiscountPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate);
        }
    }

}
