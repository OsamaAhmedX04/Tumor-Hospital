using FluentValidation;
using TumorHospital.Application.DTOs.Request.ML;

namespace TumorHospital.Application.Validators.ML
{
    public class ExplainRequestValidator
        : AbstractValidator<ExplainRequestDto>
    {
        public ExplainRequestValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty();

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image is required.");

            When(x => x.Image != null, () =>
            {
                RuleFor(x => x.Image)
                    .Must(file => file.Length <= 10 * 1024 * 1024)
                    .WithMessage("Image size cannot exceed 10 MB.");
            });

            RuleFor(x => x.Image.Length)
                .GreaterThan(0);

            RuleFor(x => x.Image.ContentType)
                .Must(x =>
                    x == "image/jpeg" ||
                    x == "image/jpg" ||
                    x == "image/png")
                .WithMessage(
                    "Only JPG and PNG files are allowed");
        }
    }
}
