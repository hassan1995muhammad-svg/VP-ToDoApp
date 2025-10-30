using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Application.DTOs;

public class CreateTodoTaskDto
{
    public string Description { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
}
