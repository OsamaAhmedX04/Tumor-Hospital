using FluentValidation;
using TumorHospital.WebAPI.DTOs.AuthDto;

namespace TumorHospital.WebAPI.Validators.Auth
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email Is Required");

            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Current Password Is Required");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New Password Is Required");

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.OldPassword).WithMessage("New Password Should Be Not The Same As Current One")
                .When(x => !string.IsNullOrEmpty(x.OldPassword) && !string.IsNullOrEmpty(x.NewPassword));
        }
    }
}
