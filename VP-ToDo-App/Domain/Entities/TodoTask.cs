using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Domain.Entities;

public class TodoTask
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime? Deadline { get; set; }
    public Domain.Enums.TaskStatus Status { get; set; } = Domain.Enums.TaskStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsOverdue => Deadline.HasValue && Deadline.Value < DateTime.UtcNow && Status == Domain.Enums.TaskStatus.Active;
}
