using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Queries.GetTasksByStatus;

public class GetTasksByStatusQueryHandler : IRequestHandler<GetTasksByStatusQuery, IEnumerable<TodoTaskDto>>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTasksByStatusQueryHandler> _logger;

    public GetTasksByStatusQueryHandler(
        ITodoTaskRepository repository,
        IMapper mapper,
        ILogger<GetTasksByStatusQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoTaskDto>> Handle(GetTasksByStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving tasks with status: {Status}", request.Status);

        var tasks = await _repository.GetTasksByStatusAsync(request.Status, cancellationToken);
        var taskDtos = _mapper.Map<IEnumerable<TodoTaskDto>>(tasks);

        _logger.LogInformation("Retrieved {Count} tasks with status {Status}", taskDtos.Count(), request.Status);

        return taskDtos;
    }
}
