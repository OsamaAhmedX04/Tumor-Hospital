using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;

namespace TumorHospital.Application.Validators.User
{
    public class UpdatePharmacistProfileDtoValidator : AbstractValidator<UpdatePharmacistProfileDto>
    {
        public UpdatePharmacistProfileDtoValidator()
        {
            RuleFor(d => d.FirstName)
               .NotEmpty().WithMessage("First Name Is Required")
               .MaximumLength(30).WithMessage("First Name Must Be Less Than 30 Character");

            RuleFor(d => d.LastName)
                .NotEmpty().WithMessage("Last Name Is Required")
                .MaximumLength(30).WithMessage("Last Name Must Be Less Than 30 Character");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(30).WithMessage("Phone Number Should not be greater than 30 character")
                .When(p => !string.IsNullOrEmpty(p.PhoneNumber));
        }
    }
}
