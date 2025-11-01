using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VP_ToDo_App.Application.Commands.DeleteTask;
using VP_ToDo_App.Domain.Entities;
using VP_ToDo_App.Domain.Exceptions;
using VP_ToDo_App.Infrastructure.Repositories;
using Xunit;

namespace VP_ToDo_App.Tests.Application.Commands;

public class DeleteTaskCommandHandlerTests
{
    private readonly Mock<ITodoTaskRepository> _repositoryMock;
    private readonly Mock<ILogger<DeleteTaskCommandHandler>> _loggerMock;
    private readonly DeleteTaskCommandHandler _handler;

    public DeleteTaskCommandHandlerTests()
    {
        _repositoryMock = new Mock<ITodoTaskRepository>();
        _loggerMock = new Mock<ILogger<DeleteTaskCommandHandler>>();
        _handler = new DeleteTaskCommandHandler(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Task_When_Exists()
    {
        var taskId = 1;
        var task = new TodoTask { Id = taskId, Description = "Test task" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        _repositoryMock
            .Setup(r => r.DeleteAsync(task, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var command = new DeleteTaskCommand { Id = taskId };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
        _repositoryMock.Verify(r => r.DeleteAsync(task, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_TaskNotFoundException_When_Task_Does_Not_Exist()
    {
        var taskId = 999;

        _repositoryMock
            .Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TodoTask?)null);

        var command = new DeleteTaskCommand { Id = taskId };
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<TaskNotFoundException>();
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<TodoTask>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
