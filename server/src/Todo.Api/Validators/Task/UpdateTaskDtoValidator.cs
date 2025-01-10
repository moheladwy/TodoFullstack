using FluentValidation;
using Todo.Core.DTOs.TasksDtos;

namespace Todo.Api.Validators.Task;

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
  public UpdateTaskDtoValidator()
  {
    RuleFor(x => x.Id)
      .NotNull().WithMessage("Id is required.")
      .NotEmpty().WithMessage("Id cannot be empty.");

    RuleFor(x => x.Name)
      .NotNull().WithMessage("Name is required.")
      .NotEmpty().WithMessage("Name cannot be empty.")
      .MinimumLength(1).WithMessage("Name must be at least 1 character.")
      .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

    RuleFor(x => x.Description)
      .MaximumLength(500)
      .WithMessage("Description must not exceed 500 characters.");

    RuleFor(x => (int)x.Priority)
      .InclusiveBetween(0, 4).WithMessage("Priority must be between 0 and 4.");

    RuleFor(x => x.IsCompleted)
      .NotNull().WithMessage("IsCompleted is required.");
  }
}
