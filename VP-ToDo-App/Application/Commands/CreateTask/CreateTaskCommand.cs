using MediatR;
using VP_ToDo_App.Application.DTOs;

namespace VP_ToDo_App.Application.Commands.CreateTask;

public class CreateTaskCommand : IRequest<TodoTaskDto>
{
    public string Description { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
}
