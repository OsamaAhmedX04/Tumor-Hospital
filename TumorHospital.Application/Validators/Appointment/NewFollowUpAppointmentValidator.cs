using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Appointment;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.Appointment
{
    public class NewFollowUpAppointmentValidator : AbstractValidator<NewFollowUpAppointmentDto>
    {
        public NewFollowUpAppointmentValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required.");
            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required.");
            RuleFor(x => x.DayOfWeek)
                .NotEmpty().WithMessage("DayOfWeek is required.")
                .Must(day => Enum.TryParse(typeof(Day), day, true, out _))
                .WithMessage("Invalid DayOfWeek value.");
        }
    }
}
