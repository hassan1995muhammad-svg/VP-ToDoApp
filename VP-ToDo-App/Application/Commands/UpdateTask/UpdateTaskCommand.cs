using MediatR;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Application.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<TodoTaskDto>
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public Domain.Enums.TaskStatus? Status { get; set; }
}
