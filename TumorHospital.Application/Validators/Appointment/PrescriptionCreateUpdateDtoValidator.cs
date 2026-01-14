using FluentValidation;
using TumorHospital.Application.DTOs.Request.Appointment;

namespace TumorHospital.Application.Validators.Appointment
{
    public class PrescriptionCreateUpdateDtoValidator
    : AbstractValidator<PrescriptionCreateUpdateDto>
    {
        public PrescriptionCreateUpdateDtoValidator()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty();

            RuleFor(x => x.Medication)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Dosage)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.StartDate)
                .LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be before end date");
        }
    }
}
