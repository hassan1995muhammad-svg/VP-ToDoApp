using MediatR;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Application.Queries.GetTasksByStatus;

public class GetTasksByStatusQuery : IRequest<IEnumerable<TodoTaskDto>>
{
    public Domain.Enums.TaskStatus Status { get; set; }
}
