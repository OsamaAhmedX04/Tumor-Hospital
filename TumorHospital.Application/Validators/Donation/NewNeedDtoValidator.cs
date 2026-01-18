using FluentValidation;
using TumorHospital.Application.DTOs.Request.Donation;
using TumorHospital.Domain.Enums;

namespace TumorHospital.Application.Validators.Donation
{
    public class NewNeedDtoValidator : AbstractValidator<NewNeedDto>
    {
        private readonly List<string> allowedImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
        private readonly long maxImageSize = 1 * 1024 * 1024; // 1 MB
        public NewNeedDtoValidator()
        {
            RuleFor(need => need.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");

            RuleFor(need => need.CharityCategory)
                .NotEmpty().WithMessage("Charity Category is required.")
                .Must(category =>
                                category == CharityCategory.Machine.ToString()
                                || category == CharityCategory.Patient.ToString()
                                || category == CharityCategory.Tools.ToString()
                                || category == CharityCategory.Other.ToString()
                )
                .WithMessage("Invalid Charity Category.");

            RuleFor(need => need.NeedAmount)
                .NotEmpty().WithMessage("Need Amount is required.")
                .GreaterThan(0).WithMessage("Need Amount must be greater than zero.");

            RuleFor(need => need.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(need => need.Image)
                .NotEmpty().WithMessage("Image Path is required.")
                .Must(image => allowedImageExtensions.Contains(Path.GetExtension(image.FileName)))
                .WithMessage($"Invalid image format. Allowed formats are: {string.Join(',', allowedImageExtensions)}")
                .Must(image => image.Length <= maxImageSize).WithMessage("Image size must be less than 1 MB.");


        }
    }
}
