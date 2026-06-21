using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Pharmacy;

namespace TumorHospital.Application.Validators.Pharmacy
{
    public class UpdatePharmacyDtoValidator : AbstractValidator<UpdatePharmacyDto>
    {
        public UpdatePharmacyDtoValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name Is Required")
                .MaximumLength(50).WithMessage("Name Should not be greater than 50 character");

            RuleFor(p => p.Location)
                .NotEmpty().WithMessage("Location Is Required")
                .MaximumLength(250).WithMessage("Location Should not be greater than 250 character");

            RuleFor(p => p.PhoneNumber)
                .MaximumLength(30).WithMessage("Phone Number Should not be greater than 30 character")
                .When(p => !string.IsNullOrEmpty(p.PhoneNumber));
        }
    }
}
