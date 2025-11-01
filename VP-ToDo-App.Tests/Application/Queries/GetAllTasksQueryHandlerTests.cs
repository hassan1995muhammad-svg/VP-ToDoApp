using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Application.Queries.GetAllTasks;
using VP_ToDo_App.Domain.Entities;
using VP_ToDo_App.Infrastructure.Repositories;
using Xunit;

namespace VP_ToDo_App.Tests.Application.Queries;

public class GetAllTasksQueryHandlerTests
{
    private readonly Mock<ITodoTaskRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<GetAllTasksQueryHandler>> _loggerMock;
    private readonly GetAllTasksQueryHandler _handler;

    public GetAllTasksQueryHandlerTests()
    {
        _repositoryMock = new Mock<ITodoTaskRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<GetAllTasksQueryHandler>>();
        _handler = new GetAllTasksQueryHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Tasks()
    {
        var tasks = new List<TodoTask>
        {
            new TodoTask { Id = 1, Description = "Task 1" },
            new TodoTask { Id = 2, Description = "Task 2" }
        };

        var taskDtos = new List<TodoTaskDto>
        {
            new TodoTaskDto { Id = 1, Description = "Task 1" },
            new TodoTaskDto { Id = 2, Description = "Task 2" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<TodoTaskDto>>(tasks))
            .Returns(taskDtos);

        var query = new GetAllTasksQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(taskDtos);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Tasks()
    {
        var tasks = new List<TodoTask>();
        var taskDtos = new List<TodoTaskDto>();

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<TodoTaskDto>>(tasks))
            .Returns(taskDtos);

        var query = new GetAllTasksQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
