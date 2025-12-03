using FluentValidation;
using TumorHospital.Application.DTOs.Request.Auth;
using TumorHospital.Domain.Enums;


namespace TumorHospital.Application.Validators.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender Is Required")
                .Must(g => g == Gender.Male.ToString() || g == Gender.Female.ToString())
                .WithMessage("Invalid Gender Value. Only Male Or Female");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .Length(2, 20).WithMessage("First Name Should be between 5 and 20 letter");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("First Name Is Required")
                .Length(2, 20).WithMessage("First Name Should be between 5 and 20 letter");
        }
    }
}
