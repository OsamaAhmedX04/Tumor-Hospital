using FluentValidation;
using TumorHospital.Application.DTOs.Request.Medicine;

namespace TumorHospital.Application.Validators.Medicine
{
    public class UpdateMedicineDtoValidator : AbstractValidator<UpdateMedicineDto>
    {
        public UpdateMedicineDtoValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name Is Required")
            .MaximumLength(100).WithMessage("Name Should Be Less Than 100 Character");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description Should Be Less Than 1000 Character")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.SellingPrice)
                .NotEmpty().WithMessage("Selling Price Is Required")
                .GreaterThan(0).WithMessage("Selling Price Should be greater than or equal zero"); ;

            RuleFor(x => x.PurchasePrice)
                .NotEmpty().WithMessage("Purchase Price Is Required")
                .GreaterThan(0).WithMessage("Purchase Price Should be greater than or equal zero"); ;

            RuleFor(x => x.SellingPrice)
                .GreaterThanOrEqualTo(x => x.PurchasePrice)
                .WithMessage("Selling price must be greater than or equal to purchase price.");

            RuleFor(x => x.QuantityInStock)
                .NotEmpty().WithMessage("Quantity In Stock Is Required")
                .GreaterThanOrEqualTo(0).WithMessage("Quantity In Stock Should be greater than or equal zero");

            RuleFor(x => x.MinimumQuantity)
                .NotEmpty().WithMessage("Minimum Quantity Is Required")
                .GreaterThanOrEqualTo(0).WithMessage("Minimum Quantity Should be greater than or equal zero");
        }
    }
}
