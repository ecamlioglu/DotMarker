using DotMarker.Domain.Entities;
using FluentValidation;

namespace DotMarker.Application.Validations;

public class ContentValidator : AbstractValidator<Content>
{
    public ContentValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required.");
    }
}