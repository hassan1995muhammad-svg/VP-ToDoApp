using FluentValidation;
using VP_ToDo_App.Application.DTOs;

namespace VP_ToDo_App.Application.Validators;

public class CreateTodoTaskValidator : AbstractValidator<CreateTodoTaskDto>
{
    public CreateTodoTaskValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Task description is required.")
            .MinimumLength(10).WithMessage("Task description must be at least 10 characters long.")
            .MaximumLength(500).WithMessage("Task description cannot exceed 500 characters.");

        RuleFor(x => x.Deadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.Deadline.HasValue)
            .WithMessage("Deadline cannot be in the past.");
    }
}
