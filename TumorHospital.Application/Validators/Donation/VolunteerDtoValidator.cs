using FluentValidation;
using TumorHospital.Application.DTOs.Request.Donation;

namespace TumorHospital.Application.Validators.Donation
{
    public class VolunteerDtoValidator : AbstractValidator<VolunteerDto>
    {
        public VolunteerDtoValidator()
        {
            RuleFor(volunteer => volunteer.VolunteerName)
                .NotEmpty().WithMessage("Volunteer Name is required.")
                .MaximumLength(100).WithMessage("Volunteer Name cannot exceed 100 characters.");

            RuleFor(volunteer => volunteer.Email)
                .EmailAddress().WithMessage("Invalid Email format.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .When(volunteer => !string.IsNullOrEmpty(volunteer.Email));

            RuleFor(volunteer => volunteer.Phone)
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid Phone number format.")
                .When(volunteer => !string.IsNullOrEmpty(volunteer.Phone));

            RuleFor(volunteer => volunteer.AmountDonated)
                .NotEmpty().WithMessage("Amount Donated is required.")
                .GreaterThan(0).WithMessage("Amount Donated must be greater than zero.");

            RuleFor(volunteer => volunteer.CharityNeedId)
                .NotEmpty().WithMessage("Charity Need Id is required.");

        }
    }
}
