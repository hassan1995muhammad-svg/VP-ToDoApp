using FluentAssertions;
using VP_ToDo_App.Domain.Entities;
using Xunit;
using TaskStatus = VP_ToDo_App.Domain.Enums.TaskStatus;

namespace VP_ToDo_App.Tests.Domain;

public class TodoTaskTests
{
    [Fact]
    public void TodoTask_Should_Have_Default_Values()
    {
        var task = new TodoTask();

        task.Description.Should().BeEmpty();
        task.Status.Should().Be(TaskStatus.Active);
        task.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        task.UpdatedAt.Should().BeNull();
        task.Deadline.Should().BeNull();
    }

    [Fact]
    public void IsOverdue_Should_Return_True_When_Deadline_Is_Past_And_Active()
    {
        var task = new TodoTask
        {
            Deadline = DateTime.UtcNow.AddDays(-1),
            Status = TaskStatus.Active
        };

        task.IsOverdue.Should().BeTrue();
    }

    [Fact]
    public void IsOverdue_Should_Return_False_When_Deadline_Is_Future()
    {
        var task = new TodoTask
        {
            Deadline = DateTime.UtcNow.AddDays(1),
            Status = TaskStatus.Active
        };

        task.IsOverdue.Should().BeFalse();
    }

    [Fact]
    public void IsOverdue_Should_Return_False_When_Task_Is_Completed()
    {
        var task = new TodoTask
        {
            Deadline = DateTime.UtcNow.AddDays(-1),
            Status = TaskStatus.Completed
        };

        task.IsOverdue.Should().BeFalse();
    }

    [Fact]
    public void IsOverdue_Should_Return_False_When_Deadline_Is_Null()
    {
        var task = new TodoTask
        {
            Deadline = null,
            Status = TaskStatus.Active
        };

        task.IsOverdue.Should().BeFalse();
    }
}
