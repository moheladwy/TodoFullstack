using FluentValidation;
using Todo.Core.DTOs.ListDTOs;

namespace Todo.Api.Validators.List;

public class UpdateListDtoValidator : AbstractValidator<UpdateListDto>
{
    public UpdateListDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("The field Id must not be null.")
            .NotEmpty().WithMessage("The field Id must not be empty.");

        RuleFor(x => x.Name)
            .NotNull().WithMessage("The field Name must not be null.")
            .NotEmpty().WithMessage("The field Name must not be empty.")
            .Length(1, 100).WithMessage("The field Name must be between 1 and 100 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("The field Description must not exceed 500 characters.");
    }
}