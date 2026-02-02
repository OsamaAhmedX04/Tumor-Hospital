using FluentValidation;
using TumorHospital.Application.DTOs.Request.About_Contact;

namespace TumorHospital.Application.Validators.About
{
    public class AddAboutInfoDtoValidator : AbstractValidator<AddAboutInfoDto>
    {
        public AddAboutInfoDtoValidator()
        {
            RuleFor(x => x.HospitalName)
                .NotEmpty().WithMessage("Hospital name is required")
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1000);

            RuleFor(x => x.Mission)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Vision)
                .NotEmpty()
                .MaximumLength(500);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
