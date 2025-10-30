using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Application.DTOs;

public class TodoTaskDto
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
    public Domain.Enums.TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsOverdue { get; set; }
}
