using MediatR;

namespace VP_ToDo_App.Application.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest<bool>
{
    public int Id { get; set; }
}
