using FluentValidation;
using Todo.Core.DTOs.AccountDTOs;

namespace Todo.Api.Validators.Account;

public class UserDtoValidator : AbstractValidator<UserDto>
{
  public UserDtoValidator()
  {
    RuleFor(x => x.Id)
    .NotNull().WithMessage("Id is required.")
    .NotEmpty().WithMessage("Id cannot be empty.");

    RuleFor(x => x.FirstName)
    .NotNull().WithMessage("First name is required.")
    .NotEmpty().WithMessage("First name cannot be empty.");

    RuleFor(x => x.LastName)
    .NotNull().WithMessage("Last name is required.")
    .NotEmpty().WithMessage("Last name cannot be empty.");

    RuleFor(x => x.Email)
    .NotNull().WithMessage("Email is required.")
    .NotEmpty().WithMessage("Email cannot be empty.")
    .EmailAddress().WithMessage("Email is not valid.");

    RuleFor(x => x.UserName)
    .NotNull().WithMessage("Username is required.")
    .NotEmpty().WithMessage("Username cannot be empty.");
  }
}
