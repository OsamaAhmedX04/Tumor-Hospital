using FluentValidation;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.User
{
    public class NewReceptionistDtoValidator : AbstractValidator<NewReceptionistDto>
    {
        public NewReceptionistDtoValidator()
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
                .Must(g => g == Gender.Male.ToString() || g == Gender.Female.ToString()).WithMessage("Only Male Or Female");

            RuleFor(d => d.Address)
                .NotEmpty().WithMessage("Address Is Required")
                .MaximumLength(100).WithMessage("Address Must Be Less Than 100 Character");
        }
    }
}
