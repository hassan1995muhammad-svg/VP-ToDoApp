using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using VP_ToDo_App.Application.Commands.CreateTask;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Domain.Entities;
using VP_ToDo_App.Infrastructure.Repositories;
using Xunit;
using TaskStatus = VP_ToDo_App.Domain.Enums.TaskStatus;

namespace VP_ToDo_App.Tests.Application.Commands;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<ITodoTaskRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IValidator<CreateTodoTaskDto>> _validatorMock;
    private readonly Mock<ILogger<CreateTaskCommandHandler>> _loggerMock;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITodoTaskRepository>();
        _mapperMock = new Mock<IMapper>();
        _validatorMock = new Mock<IValidator<CreateTodoTaskDto>>();
        _loggerMock = new Mock<ILogger<CreateTaskCommandHandler>>();

        _handler = new CreateTaskCommandHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Create_Task_When_Valid()
    {
        var command = new CreateTaskCommand
        {
            Description = "Test task description",
            Deadline = DateTime.UtcNow.AddDays(1)
        };

        var task = new TodoTask
        {
            Id = 1,
            Description = command.Description,
            Deadline = command.Deadline,
            CreatedAt = DateTime.UtcNow
        };

        var taskDto = new TodoTaskDto
        {
            Id = 1,
            Description = task.Description,
            Deadline = task.Deadline,
            Status = TaskStatus.Active
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTodoTaskDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mapperMock
            .Setup(m => m.Map<TodoTask>(It.IsAny<CreateTodoTaskDto>()))
            .Returns(task);

        _mapperMock
            .Setup(m => m.Map<TodoTaskDto>(It.IsAny<TodoTask>()))
            .Returns(taskDto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<TodoTask>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Description.Should().Be(command.Description);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TodoTask>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_ValidationException_When_Invalid()
    {
        var command = new CreateTaskCommand
        {
            Description = "Short",
            Deadline = DateTime.UtcNow.AddDays(1)
        };

        var validationFailures = new List<ValidationFailure>
        {
            new ValidationFailure("Description", "Task description must be at least 10 characters long.")
        };

        _validatorMock
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTodoTaskDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(validationFailures));

        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<TodoTask>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
