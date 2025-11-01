using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using VP_ToDo_App.Domain.Entities;
using VP_ToDo_App.Infrastructure.Data;
using VP_ToDo_App.Infrastructure.Repositories;
using Xunit;
using TaskStatus = VP_ToDo_App.Domain.Enums.TaskStatus;

namespace VP_ToDo_App.Tests.Infrastructure.Repositories;

public class TodoTaskRepositoryTests
{
    private TodoDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new TodoDbContext(options);
    }

    [Fact]
    public async Task AddAsync_Should_Add_Task_To_Database()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        var task = new TodoTask
        {
            Description = "Test task description",
            Status = TaskStatus.Active
        };

        await repository.AddAsync(task);
        await repository.SaveChangesAsync();

        var tasks = await context.Tasks.ToListAsync();
        tasks.Should().HaveCount(1);
        tasks[0].Description.Should().Be("Test task description");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Task_When_Exists()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        var task = new TodoTask { Description = "Test task" };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        var result = await repository.GetByIdAsync(task.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(task.Id);
        result.Description.Should().Be("Test task");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        var result = await repository.GetByIdAsync(999);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GetTasksByStatusAsync_Should_Return_Only_Active_Tasks()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        context.Tasks.AddRange(
            new TodoTask { Description = "Active task 1", Status = TaskStatus.Active },
            new TodoTask { Description = "Active task 2", Status = TaskStatus.Active },
            new TodoTask { Description = "Completed task", Status = TaskStatus.Completed }
        );
        await context.SaveChangesAsync();

        var result = await repository.GetTasksByStatusAsync(TaskStatus.Active);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.Status == TaskStatus.Active);
    }

    [Fact]
    public async Task SearchTasksAsync_Should_Return_Matching_Tasks()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        context.Tasks.AddRange(
            new TodoTask { Description = "Buy groceries from store" },
            new TodoTask { Description = "Clean the house" },
            new TodoTask { Description = "Buy new shoes" }
        );
        await context.SaveChangesAsync();

        var result = await repository.SearchTasksAsync("Buy");

        result.Should().HaveCount(2);
        result.Should().OnlyContain(t => t.Description.Contains("Buy"));
    }

    [Fact]
    public async Task GetOverdueTasksAsync_Should_Return_Only_Overdue_Active_Tasks()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        context.Tasks.AddRange(
            new TodoTask
            {
                Description = "Overdue active task",
                Deadline = DateTime.UtcNow.AddDays(-1),
                Status = TaskStatus.Active
            },
            new TodoTask
            {
                Description = "Future task",
                Deadline = DateTime.UtcNow.AddDays(1),
                Status = TaskStatus.Active
            },
            new TodoTask
            {
                Description = "Overdue completed task",
                Deadline = DateTime.UtcNow.AddDays(-1),
                Status = TaskStatus.Completed
            }
        );
        await context.SaveChangesAsync();

        var result = await repository.GetOverdueTasksAsync();

        result.Should().HaveCount(1);
        result.First().Description.Should().Be("Overdue active task");
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_Task()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        var task = new TodoTask { Description = "Original description" };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        task.Description = "Updated description";
        task.UpdatedAt = DateTime.UtcNow;
        await repository.UpdateAsync(task);
        await repository.SaveChangesAsync();

        var updatedTask = await context.Tasks.FindAsync(task.Id);
        updatedTask!.Description.Should().Be("Updated description");
        updatedTask.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task DeleteAsync_Should_Remove_Task()
    {
        using var context = CreateDbContext();
        var repository = new TodoTaskRepository(context);

        var task = new TodoTask { Description = "Task to delete" };
        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        await repository.DeleteAsync(task);
        await repository.SaveChangesAsync();

        var tasks = await context.Tasks.ToListAsync();
        tasks.Should().BeEmpty();
    }
}
