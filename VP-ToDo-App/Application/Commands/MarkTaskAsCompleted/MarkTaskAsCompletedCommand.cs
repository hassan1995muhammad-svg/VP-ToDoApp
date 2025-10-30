using MediatR;

namespace VP_ToDo_App.Application.Commands.MarkTaskAsCompleted;

public class MarkTaskAsCompletedCommand : IRequest<bool>
{
    public int Id { get; set; }
}
