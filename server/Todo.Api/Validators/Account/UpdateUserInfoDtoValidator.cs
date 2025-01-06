using System;
using FluentValidation;
using Todo.Core.DTOs.AccountDTOs;

namespace Todo.Api.Validators.Account;

public class UpdateUserInfoDtoValidator : AbstractValidator<UpdateUserInfoDto>
{
  public UpdateUserInfoDtoValidator()
  {
    RuleFor(x => x.Id)
      .NotNull().WithMessage("Id is required.")
      .NotEmpty().WithMessage("Id cannot be empty.");

    RuleFor(x => x.NewFirstName)
      .MinimumLength(3).WithMessage("First name must be at least 3 characters.")
      .MaximumLength(25).WithMessage("First name must not exceed 25 characters.");

    RuleFor(x => x.NewLastName)
      .MinimumLength(3).WithMessage("Last name must be at least 3 characters.")
      .MaximumLength(25).WithMessage("Last name must not exceed 25 characters.");

    RuleFor(x => x.NewEmail)
      .EmailAddress().WithMessage("Invalid email address.");

    RuleFor(x => x.NewUserName)
      .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
      .MaximumLength(25).WithMessage("Username must not exceed 25 characters.");

    RuleFor(x => x.NewPhoneNumber)
      .Custom((phoneNumber, context) =>
      {
        if (phoneNumber != null)
        {
          if (phoneNumber.Length < 10 || phoneNumber.Length > 15)
          {
            context.AddFailure("Phone number must be between 10 and 15 characters.");
          }
        }
      });
  }
}
