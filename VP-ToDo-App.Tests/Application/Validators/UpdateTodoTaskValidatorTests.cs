using FluentAssertions;
using FluentValidation.TestHelper;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Application.Validators;
using Xunit;
using TaskStatus = VP_ToDo_App.Domain.Enums.TaskStatus;

namespace VP_ToDo_App.Tests.Application.Validators;

public class UpdateTodoTaskValidatorTests
{
    private readonly UpdateTodoTaskValidator _validator;

    public UpdateTodoTaskValidatorTests()
    {
        _validator = new UpdateTodoTaskValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Short()
    {
        var dto = new UpdateTodoTaskDto { Description = "Short" };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        var dto = new UpdateTodoTaskDto { Description = new string('a', 501) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Deadline_Is_In_Past()
    {
        var dto = new UpdateTodoTaskDto
        {
            Deadline = DateTime.UtcNow.AddDays(-1)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Deadline);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
    {
        var dto = new UpdateTodoTaskDto
        {
            Description = "Valid updated description",
            Deadline = DateTime.UtcNow.AddDays(1),
            Status = TaskStatus.Active
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Fields_Are_Null()
    {
        var dto = new UpdateTodoTaskDto
        {
            Description = null,
            Deadline = null,
            Status = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
