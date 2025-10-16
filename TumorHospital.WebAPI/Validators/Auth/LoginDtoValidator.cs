using FluentValidation;
using TumorHospital.WebAPI.DTOs.AuthDto;

namespace TumorHospital.WebAPI.Validators.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required");
        }
    }
}
