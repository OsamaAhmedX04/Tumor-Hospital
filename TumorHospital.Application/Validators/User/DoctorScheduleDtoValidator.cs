using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;

namespace TumorHospital.Application.Validators.User
{
    public class DoctorScheduleDtoValidator : AbstractValidator<DoctorScheduleDto>
    {
        private readonly List<string> AvailableDays
            = ["Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday"];
        public DoctorScheduleDtoValidator()
        {
            RuleFor(d => d.DayOfWeek)
                .NotEmpty().WithMessage("Please Enter Work Day")
                .NotEqual("Friday").WithMessage("It's The Holiday")
                .Must(dayOfWeek => AvailableDays.Contains(dayOfWeek)).WithMessage("Please Enter Day Of week");

            RuleFor(d => d.StartTime)
                .NotEmpty().WithMessage("Please Enter Start Time")
                .GreaterThanOrEqualTo(new TimeSpan(6, 0, 0)).WithMessage("Hospital Start Time in 6 AM")
                .LessThanOrEqualTo(new TimeSpan(17, 0, 0)).WithMessage("Hospital Close At 1 and Doctors work time is 8 hours");
        }
    }
}
