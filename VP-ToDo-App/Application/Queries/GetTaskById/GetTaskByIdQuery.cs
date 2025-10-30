using MediatR;
using VP_ToDo_App.Application.DTOs;

namespace VP_ToDo_App.Application.Queries.GetTaskById;

public class GetTaskByIdQuery : IRequest<TodoTaskDto?>
{
    public int Id { get; set; }
}
