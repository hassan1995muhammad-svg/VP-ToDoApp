using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Queries.GetAllTasks;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TodoTaskDto>>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllTasksQueryHandler> _logger;

    public GetAllTasksQueryHandler(
        ITodoTaskRepository repository,
        IMapper mapper,
        ILogger<GetAllTasksQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoTaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all tasks");

        var tasks = await _repository.GetAllAsync(cancellationToken);
        var taskDtos = _mapper.Map<IEnumerable<TodoTaskDto>>(tasks);

        _logger.LogInformation("Retrieved {Count} tasks", taskDtos.Count());

        return taskDtos;
    }
}
