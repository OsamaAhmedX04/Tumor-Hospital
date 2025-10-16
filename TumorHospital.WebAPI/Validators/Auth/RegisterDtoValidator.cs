using FluentValidation;
using TumorHospital.WebAPI.DTOs.AuthDto;

namespace TumorHospital.WebAPI.Validators.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .Length(5, 20).WithMessage("First Name Should be between 5 and 20 letter");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("First Name Is Required")
                .Length(5, 20).WithMessage("First Name Should be between 5 and 20 letter");
        }
    }
}
