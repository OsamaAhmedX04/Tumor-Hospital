using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Hospital;

namespace TumorHospital.Application.Validators.Hospital
{
    public class UpdateHospitalDtoValidator : AbstractValidator<UpdateHospitalDto>
    {
        public UpdateHospitalDtoValidator()
        {
            RuleFor(x => x.Government)
                .NotEmpty().WithMessage("Government is required.")
                .MaximumLength(100).WithMessage("Government must not exceed 100 characters.");

            RuleFor(x => x.MaxNumberOfDoctors)
                .GreaterThan(0).WithMessage("Max number of doctors must be greater than zero.")
                .LessThanOrEqualTo(200).WithMessage("Max number of doctors must be less than 200.");

            RuleFor(x => x.MaxNumberOfReceptionists)
                .GreaterThan(0).WithMessage("Max number of receptionists must be greater than zero.")
                .LessThanOrEqualTo(200).WithMessage("Max number of receptionists must be less than 200.");
        }
    }
}
