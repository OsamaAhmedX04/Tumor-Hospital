using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;

namespace TumorHospital.Application.Validators.User
{
    public class DoctorScheduleDtoValidator : AbstractValidator<DoctorScheduleDto>
    {
        private readonly List<string> AvailableDays
            = ["Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"];
        public DoctorScheduleDtoValidator()
        {
            RuleFor(d => d.DayOfWeek)
                .NotEmpty().WithMessage("Please Enter Work Day")
                .Must(dayOfWeek => AvailableDays.Contains(dayOfWeek)).WithMessage("Please Enter Day Of week");

            RuleFor(d => d.StartTime)
                .NotEmpty().WithMessage("Please Enter Start Time");

        }
    }
}
