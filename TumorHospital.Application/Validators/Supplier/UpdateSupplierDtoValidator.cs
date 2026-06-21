using FluentValidation;
using TumorHospital.Application.DTOs.Request.Supply;

namespace TumorHospital.Application.Validators.Supplier
{
    public class UpdateSupplierDtoValidator : AbstractValidator<UpdateSupplierDto>
    {
        public UpdateSupplierDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name Is Required")
                .MaximumLength(50).WithMessage("Name Should not be greater than 50 character");

            RuleFor(p => p.Address)
                .NotEmpty().WithMessage("Address Is Required")
                .MaximumLength(250).WithMessage("Address Should not be greater than 250 character");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(30).WithMessage("Phone Number Should not be greater than 30 character")
                .When(p => !string.IsNullOrEmpty(p.PhoneNumber));

            RuleFor(p => p.ContactPersonName)
                .MaximumLength(50).WithMessage("Contact Person Name Should not be greater than 50 character")
                .When(p => !string.IsNullOrEmpty(p.ContactPersonName));

            RuleFor(p => p.ContactPersonPhone)
                .MaximumLength(30).WithMessage("Contact Person Phone Number Should not be greater than 30 character")
                .When(p => !string.IsNullOrEmpty(p.ContactPersonPhone));
        }
    }
}
