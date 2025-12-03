using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.Appointment;

namespace TumorHospital.Application.Validators.Appointment
{
    public class AppointmentSetterDateTimeDtoValidator : AbstractValidator<AppointmentSetterDateTimeDto>
    {
        public AppointmentSetterDateTimeDtoValidator()
        {
            RuleFor(x => x.FromTime)
                .NotEmpty()
                .WithMessage("FromTime is required.");
            RuleFor(x => x.ToTime)
                .NotEmpty()
                .WithMessage("ToTime is required.");
            RuleFor(x => x.AttendenceDate)
                .NotEmpty()
                .WithMessage("AttendenceDate is required.");

            RuleFor(x => x.FromTime)
                .LessThan(x => x.ToTime)
                .WithMessage("FromTime must be earlier than ToTime.");

            RuleFor(x => x.AttendenceDate)
                .GreaterThan(DateTime.Now)
                .WithMessage("AttendenceDate must be a future date.");
        }
    }
}
