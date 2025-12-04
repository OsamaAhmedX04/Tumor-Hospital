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
            
        }
    }
}
