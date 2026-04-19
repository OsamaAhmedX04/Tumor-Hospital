using FluentValidation;
using TumorHospital.Application.DTOs.Request.Auth;

namespace TumorHospital.Application.Validators.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password Is Required");

            RuleFor(x => x.RememberMe)
                .NotEmpty().WithMessage("Remember Me Is Required (True/False)");
        }
    }
}
