using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.FAQs;

namespace TumorHospital.Application.Validators.FAQ
{
    public class NewFAQsDtoValidator : AbstractValidator<NewFAQsDto>
    {
        public NewFAQsDtoValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("Question is required.")
                .MaximumLength(500).WithMessage("Question cannot exceed 500 characters.");

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("Answer is required.")
                .MaximumLength(2000).WithMessage("Answer cannot exceed 2000 characters.");
        }
    }
}
