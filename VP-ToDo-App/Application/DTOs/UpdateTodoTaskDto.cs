using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Application.DTOs;

public class UpdateTodoTaskDto
{
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public Domain.Enums.TaskStatus? Status { get; set; }
}
