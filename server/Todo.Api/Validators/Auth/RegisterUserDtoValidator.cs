using System;
using FluentValidation;
using Todo.Core.DTOs.AuthDTOs;

namespace Todo.Api.Validators.Auth;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
  public RegisterUserDtoValidator()
  {
    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Email is not valid.");

    RuleFor(x => x.Username)
        .NotNull().WithMessage("Username is required.")
        .NotEmpty().WithMessage("Username cannot be empty.")
        .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
        .MaximumLength(25).WithMessage("Username must not exceed 25 characters.");

    RuleFor(x => x.FirstName)
        .NotNull().WithMessage("First name is required.")
        .NotEmpty().WithMessage("First name cannot be empty.")
        .MinimumLength(3).WithMessage("First name must be at least 3 characters long.")
        .MaximumLength(25).WithMessage("First name must not exceed 25 characters.");

    RuleFor(x => x.LastName)
        .NotNull().WithMessage("Last name is required.")
        .NotEmpty().WithMessage("Last name cannot be empty.")
        .MinimumLength(3).WithMessage("Last name must be at least 3 characters long.")
        .MaximumLength(25).WithMessage("Last name must not exceed 25 characters.");

    RuleFor(x => x.Password)
        .NotNull().WithMessage("Password is required.")
        .NotEmpty().WithMessage("Password cannot be empty.")
        .MinimumLength(12).WithMessage("Password must be at least 12 characters long.")
        .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
        .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$")
        .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
  }
}
