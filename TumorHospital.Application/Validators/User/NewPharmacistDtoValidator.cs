using FluentValidation;
using TumorHospital.Application.DTOs.Request.Pharmacy;

namespace TumorHospital.Application.Validators.User
{
    public class NewPharmacistDtoValidator : AbstractValidator<NewPharmacistDto>
    {
        public NewPharmacistDtoValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .MaximumLength(50).WithMessage("First Name Should not be greater than 50 character");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last Name Is Required")
                .MaximumLength(50).WithMessage("Last Name Should not be greater than 50 character");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Please Enter Correct Email Format");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(30).WithMessage("Phone Number Should not be greater than 30 character")
                .When(p => !string.IsNullOrEmpty(p.PhoneNumber));

            RuleFor(p => p.PharmacyId)
                .NotEmpty().WithMessage("Pharmacy Id Is Required");
        }
    }
}
