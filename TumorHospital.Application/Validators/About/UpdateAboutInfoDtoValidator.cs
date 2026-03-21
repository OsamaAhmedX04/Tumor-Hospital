using FluentValidation;
using TumorHospital.Application.DTOs.Request.About_Contact;

namespace TumorHospital.Application.Validators.About
{
    public class UpdateAboutInfoDtoValidator : AbstractValidator<UpdateAboutInfoDto>
    {
        public UpdateAboutInfoDtoValidator()
        {
            RuleFor(x => x.HospitalName)
                .NotEmpty().WithMessage("Hospital name is required")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Hospital Description is required")
                .MaximumLength(1000);

            RuleFor(x => x.Mission)
                .NotEmpty().WithMessage("Hospital Mission is required")
                .MaximumLength(500);

            RuleFor(x => x.Vision)
                .NotEmpty().WithMessage("Hospital Vision is required")
                .MaximumLength(500);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Hospital Email is required")
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Hospital Phone is required")
                .MaximumLength(50);
        }
    }
}
