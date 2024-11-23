using DotMarker.Domain.Entities;
using FluentValidation;

namespace DotMarker.Application.Validations;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().WithMessage("Full name is required.");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Invalid email format.");
    }
}