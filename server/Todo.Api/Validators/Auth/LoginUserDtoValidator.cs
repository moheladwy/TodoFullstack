using System;
using FluentValidation;
using Todo.Core.DTOs.AuthDTOs;

namespace Todo.Api.Validators.Auth;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
  public LoginUserDtoValidator()
  {
    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email is not valid.");

    RuleFor(x => x.Password)
        .NotNull().WithMessage("Password is required.")
        .NotEmpty().WithMessage("Password cannot be empty.");
  }
}
