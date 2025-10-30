using VP_ToDo_App.Domain.Entities;

namespace VP_ToDo_App.Infrastructure.Repositories;

public interface ITodoTaskRepository : IRepository<TodoTask>
{
    Task<IEnumerable<TodoTask>> GetTasksByStatusAsync(Domain.Enums.TaskStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<TodoTask>> SearchTasksAsync(string searchQuery, CancellationToken cancellationToken = default);
    Task<IEnumerable<TodoTask>> GetOverdueTasksAsync(CancellationToken cancellationToken = default);
}
