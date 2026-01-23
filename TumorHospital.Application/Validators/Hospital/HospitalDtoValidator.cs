using FluentValidation;
using TumorHospital.Application.DTOs.Request.Hospital;

namespace TumorHospital.Application.Validators.Hospital
{
    public class HospitalDtoValidator : AbstractValidator<HospitalDto>
    {
        public HospitalDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Hospital name is required.")
                .MaximumLength(100).WithMessage("Hospital name must not exceed 100 characters.");

            RuleFor(x => x.Government)
                .NotEmpty().WithMessage("Government is required.")
                .MaximumLength(100).WithMessage("Government must not exceed 100 characters.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(300).WithMessage("Address must not exceed 300 characters.");

            RuleFor(x => x.MaxNumberOfDoctors)
                .GreaterThan(0).WithMessage("Max number of doctors must be greater than zero.")
                .LessThanOrEqualTo(200).WithMessage("Max number of doctors must be less than 200.");

            RuleFor(x => x.MaxNumberOfReceptionists)
                .GreaterThan(0).WithMessage("Max number of receptionists must be greater than zero.")
                .LessThanOrEqualTo(200).WithMessage("Max number of receptionists must be less than 200.");
        }
    }
}
