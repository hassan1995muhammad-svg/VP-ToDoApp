using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Domain.Exceptions;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Commands.MarkTaskAsCompleted;

public class MarkTaskAsCompletedCommandHandler : IRequestHandler<MarkTaskAsCompletedCommand, bool>
{
    private readonly ITodoTaskRepository _repository;
    private readonly ILogger<MarkTaskAsCompletedCommandHandler> _logger;

    public MarkTaskAsCompletedCommandHandler(
        ITodoTaskRepository repository,
        ILogger<MarkTaskAsCompletedCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(MarkTaskAsCompletedCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marking task with ID {TaskId} as completed", request.Id);

        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", request.Id);
            throw new TaskNotFoundException(request.Id);
        }

        task.Status = Domain.Enums.TaskStatus.Completed;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task with ID {TaskId} marked as completed", request.Id);

        return true;
    }
}
