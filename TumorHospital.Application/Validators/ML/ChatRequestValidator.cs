using FluentValidation;
using TumorHospital.Application.DTOs.Request.ML;

namespace TumorHospital.Application.Validators.ML
{

    public class ChatRequestValidator
        : AbstractValidator<ChatRequestDto>
    {
        public ChatRequestValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}
