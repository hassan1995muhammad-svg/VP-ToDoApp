using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Domain.Exceptions;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITodoTaskRepository _repository;
    private readonly ILogger<DeleteTaskCommandHandler> _logger;

    public DeleteTaskCommandHandler(
        ITodoTaskRepository repository,
        ILogger<DeleteTaskCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting task with ID: {TaskId}", request.Id);

        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", request.Id);
            throw new TaskNotFoundException(request.Id);
        }

        await _repository.DeleteAsync(task, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task with ID {TaskId} deleted successfully", request.Id);

        return true;
    }
}
