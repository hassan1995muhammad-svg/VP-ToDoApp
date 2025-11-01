using FluentAssertions;
using FluentValidation.TestHelper;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Application.Validators;
using Xunit;

namespace VP_ToDo_App.Tests.Application.Validators;

public class CreateTodoTaskValidatorTests
{
    private readonly CreateTodoTaskValidator _validator;

    public CreateTodoTaskValidatorTests()
    {
        _validator = new CreateTodoTaskValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var dto = new CreateTodoTaskDto { Description = "" };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Short()
    {
        var dto = new CreateTodoTaskDto { Description = "Short" };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Task description must be at least 10 characters long.");
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Too_Long()
    {
        var dto = new CreateTodoTaskDto { Description = new string('a', 501) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Task description cannot exceed 500 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Deadline_Is_In_Past()
    {
        var dto = new CreateTodoTaskDto
        {
            Description = "Valid description with enough characters",
            Deadline = DateTime.UtcNow.AddDays(-1)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Deadline);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new CreateTodoTaskDto
        {
            Description = "Valid task description",
            Deadline = DateTime.UtcNow.AddDays(1)
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_Deadline_Is_Null()
    {
        var dto = new CreateTodoTaskDto
        {
            Description = "Valid task description",
            Deadline = null
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
