using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.User
{
    public class NewDoctorDtoValidator : AbstractValidator<NewDoctorDto>
    {
        public NewDoctorDtoValidator()
        {
            RuleFor(d => d.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .MaximumLength(30).WithMessage("First Name Must Be Less Than 30 Character");

            RuleFor(d => d.LastName)
                .NotEmpty().WithMessage("Last Name Is Required")
                .MaximumLength(30).WithMessage("Last Name Must Be Less Than 30 Character");

            RuleFor(d => d.Email)
                .NotEmpty().WithMessage("Email Is Required")
                .EmailAddress().WithMessage("Please Enter Right Email");

            RuleFor(d => d.Gender)
                .NotEmpty().WithMessage("Please Enter The Gender")
                .Must(g => g == Gender.Male.ToString() || g == Gender.Male.ToString()).WithMessage("Only Male Or Female");

            RuleFor(d => d.SpecializationName)
                .NotEmpty().WithMessage("Specialization Is Required")
                .MaximumLength(100);


            RuleForEach(d => d.Schedules)
                .SetValidator(new DoctorScheduleDtoValidator())
                .When(d => d.Schedules != null && d.Schedules.Any());

            RuleFor(d => d.Schedules)
                .NotEmpty().WithMessage("The Schedules are Required");

            RuleFor(d => d.Schedules)
                .Must(list => list.Count >= 3 && list.Count <= 5).WithMessage("Only 3 to 5 Working Days Per Week")
                .Must(list => list.DistinctBy(l => l.DayOfWeek).Count() == list.Count()).WithMessage("Duplication of Days is not allowed");
        }
    }
}
