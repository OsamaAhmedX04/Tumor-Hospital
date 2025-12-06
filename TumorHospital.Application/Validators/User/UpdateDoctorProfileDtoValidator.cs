using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.User
{
    public class UpdateDoctorProfileDtoValidator : AbstractValidator<UpdateDoctorProfileDto>
    {
        public UpdateDoctorProfileDtoValidator()
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

            RuleFor(x => x.Bio).MaximumLength(500);
        }
    }

}
