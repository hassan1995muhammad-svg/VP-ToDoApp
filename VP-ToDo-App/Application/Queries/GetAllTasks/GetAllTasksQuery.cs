using MediatR;
using VP_ToDo_App.Application.DTOs;

namespace VP_ToDo_App.Application.Queries.GetAllTasks;

public class GetAllTasksQuery : IRequest<IEnumerable<TodoTaskDto>>
{
}
