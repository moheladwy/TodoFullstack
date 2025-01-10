using FluentValidation;
using Todo.Core.DTOs.TasksDtos;

namespace Todo.Api.Validators.Task;

public class AddTaskDtoValidator : AbstractValidator<AddTaskDto>
{
  public AddTaskDtoValidator()
  {
    RuleFor(x => x.Name)
      .NotNull().WithMessage("Name is required.")
      .NotEmpty().WithMessage("Name cannot be empty.")
      .MinimumLength(1).WithMessage("Name must be at least 1 character.")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    RuleFor(x => x.Description)
      .MaximumLength(500)
      .WithMessage("Description must not exceed 500 characters.");

    RuleFor(x => x.Priority)
      .NotNull().WithMessage("Priority is required.")
      .InclusiveBetween(0, 4)
      .WithMessage("Priority must be between 0 and 4.");

    RuleFor(x => x.ListId)
      .NotNull().WithMessage("ListId is required.")
      .NotEmpty().WithMessage("ListId cannot be empty.");
  }
}
