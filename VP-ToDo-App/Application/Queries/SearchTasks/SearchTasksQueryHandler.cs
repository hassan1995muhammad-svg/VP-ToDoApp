using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Queries.SearchTasks;

public class SearchTasksQueryHandler : IRequestHandler<SearchTasksQuery, IEnumerable<TodoTaskDto>>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<SearchTasksQueryHandler> _logger;

    public SearchTasksQueryHandler(
        ITodoTaskRepository repository,
        IMapper mapper,
        ILogger<SearchTasksQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TodoTaskDto>> Handle(SearchTasksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Searching tasks with query: {SearchQuery}", request.SearchQuery);

        var tasks = await _repository.SearchTasksAsync(request.SearchQuery, cancellationToken);
        var taskDtos = _mapper.Map<IEnumerable<TodoTaskDto>>(tasks);

        _logger.LogInformation("Found {Count} tasks matching query: {SearchQuery}", taskDtos.Count(), request.SearchQuery);

        return taskDtos;
    }
}
