using FluentValidation;
using Todo.Core.DTOs.AccountDTOs;

namespace Todo.Api.Validators.Account;

public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
  public ChangePasswordDtoValidator()
  {
    RuleFor(x => x.Id)
      .NotEmpty().WithMessage("Id is required.")
      .NotNull().WithMessage("Id is required.");

    RuleFor(x => x.CurrentPassword)
      .NotEmpty().WithMessage("Current password is required.")
      .NotNull().WithMessage("Current password is required.");

    RuleFor(x => x.NewPassword)
      .NotEmpty().WithMessage("New password is required.")
      .NotNull().WithMessage("New password is required.")
      .MinimumLength(12).WithMessage("New password must be at least 12 characters long.")
      .MaximumLength(100).WithMessage("New password must not exceed 100 characters.")
      .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{12,100}$")
      .WithMessage("New password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")
      .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from the current password.");
  }
}
