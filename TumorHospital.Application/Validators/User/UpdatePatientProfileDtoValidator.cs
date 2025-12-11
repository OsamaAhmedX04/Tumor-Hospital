using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.User
{
    public class UpdatePatientProfileDtoValidator : AbstractValidator<UpdatePatientProfileDto>
    {
        public UpdatePatientProfileDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50);

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number");

            RuleFor(x => x.Gender)
                .Must(g => g == Gender.Male.ToString() || g == Gender.Female.ToString()).WithMessage("Invalid gender");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Date of birth must be in the past");
        }
    }
}
