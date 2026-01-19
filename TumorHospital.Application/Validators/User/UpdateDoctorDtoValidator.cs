using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TumorHospital.Application.DTOs.Request.User;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.User
{
    public class UpdateDoctorDtoValidator : AbstractValidator<UpdateDoctorDto>
    {
        public UpdateDoctorDtoValidator()
        {
            RuleFor(d => d.FirstName)
                .NotEmpty().WithMessage("First Name Is Required")
                .MaximumLength(30).WithMessage("First Name Must Be Less Than 30 Character");

            RuleFor(d => d.LastName)
                .NotEmpty().WithMessage("Last Name Is Required")
                .MaximumLength(30).WithMessage("Last Name Must Be Less Than 30 Character");

            RuleFor(d => d.Gender)
                .NotEmpty().WithMessage("Please Enter The Gender")
                .Must(g => g == Gender.Male.ToString() || g == Gender.Female.ToString()).WithMessage("Only Male Or Female");

            RuleFor(d => d.SpecializationName)
                .NotEmpty().WithMessage("Specialization Is Required")
                .MaximumLength(100);

            RuleFor(d => d.IsSurgeon)
                .NotNull().WithMessage("IsSurgeon Field Is Required");

            RuleFor(d => d.ConsultationCost)
                .GreaterThan(0).WithMessage("Consultation Cost Must Be Greater Than 0");

            RuleFor(d => d.FollowUpCost)
                .GreaterThan(0).WithMessage("Follow Up Cost Must Be Greater Than 0")
                .LessThanOrEqualTo(d => d.ConsultationCost).WithMessage("Follow Up Cost Must Be Less Than or equal Consultation Cost");

            RuleFor(d => d.SurgeryCost)
                .GreaterThan(0).When(d => d.IsSurgeon)
                .WithMessage("Surgery Cost Must Be Greater Than Zero")
                .GreaterThan(d => d.ConsultationCost).When(d => d.IsSurgeon)
                .WithMessage("Surgery Cost Must Be Greater Than Consultation Cost")
                .GreaterThan(d => d.FollowUpCost).When(d => d.IsSurgeon)
                .WithMessage("Surgery Cost Must Be Greater Than Follow-Up Cost");
        }
    }
}
