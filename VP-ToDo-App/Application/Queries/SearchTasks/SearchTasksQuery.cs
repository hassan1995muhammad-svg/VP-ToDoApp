using MediatR;
using VP_ToDo_App.Application.DTOs;

namespace VP_ToDo_App.Application.Queries.SearchTasks;

public class SearchTasksQuery : IRequest<IEnumerable<TodoTaskDto>>
{
    public string SearchQuery { get; set; } = string.Empty;
}
