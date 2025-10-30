using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TodoTaskDto?>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTaskByIdQueryHandler> _logger;

    public GetTaskByIdQueryHandler(
        ITodoTaskRepository repository,
        IMapper mapper,
        ILogger<GetTaskByIdQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TodoTaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving task with ID: {TaskId}", request.Id);

        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", request.Id);
            return null;
        }

        return _mapper.Map<TodoTaskDto>(task);
    }
}
