using FluentValidation;
using VP_ToDo_App.Application.DTOs;

namespace VP_ToDo_App.Application.Validators;

public class UpdateTodoTaskValidator : AbstractValidator<UpdateTodoTaskDto>
{
    public UpdateTodoTaskValidator()
    {
        RuleFor(x => x.Description)
            .MinimumLength(10).WithMessage("Task description must be at least 10 characters long.")
            .MaximumLength(500).WithMessage("Task description cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Deadline)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .When(x => x.Deadline.HasValue)
            .WithMessage("Deadline cannot be in the past.");
    }
}
