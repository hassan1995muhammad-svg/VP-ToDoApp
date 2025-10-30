using Microsoft.EntityFrameworkCore;
using VP_ToDo_App.Domain.Entities;
using VP_ToDo_App.Infrastructure.Data;

namespace VP_ToDo_App.Infrastructure.Repositories;

public class TodoTaskRepository : Repository<TodoTask>, ITodoTaskRepository
{
    private readonly TodoDbContext _context;

    public TodoTaskRepository(TodoDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoTask>> GetTasksByStatusAsync(Domain.Enums.TaskStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TodoTask>> SearchTasksAsync(string searchQuery, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchQuery))
        {
            return await GetAllAsync(cancellationToken);
        }

        return await _context.Tasks
            .Where(t => t.Description.Contains(searchQuery))
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TodoTask>> GetOverdueTasksAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        return await _context.Tasks
            .Where(t => t.Deadline.HasValue && t.Deadline.Value < now && t.Status == Domain.Enums.TaskStatus.Active)
            .OrderBy(t => t.Deadline)
            .ToListAsync(cancellationToken);
    }
}
