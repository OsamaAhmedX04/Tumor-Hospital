using FluentValidation;
using TumorHospital.Application.DTOs.Request.Offer;
namespace TumorHospital.Application.Validators.Offer
{

    public class AddOfferDtoValidator : AbstractValidator<AddOfferDto>
    {
        public AddOfferDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().MaximumLength(100);

            RuleFor(x => x.DiscountPercentage)
                .GreaterThan(0)
                .LessThanOrEqualTo(100);

            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("Start date must be before end date");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage("End date must be in the future");
        }
    }

}
