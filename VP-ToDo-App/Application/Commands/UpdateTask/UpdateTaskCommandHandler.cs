using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Domain.Exceptions;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Commands.UpdateTask;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TodoTaskDto>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UpdateTodoTaskDto> _validator;
    private readonly ILogger<UpdateTaskCommandHandler> _logger;

    public UpdateTaskCommandHandler(
        ITodoTaskRepository repository,
        IMapper mapper,
        IValidator<UpdateTodoTaskDto> validator,
        ILogger<UpdateTaskCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task<TodoTaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating task with ID: {TaskId}", request.Id);

        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {TaskId} not found", request.Id);
            throw new TaskNotFoundException(request.Id);
        }

        var updateDto = new UpdateTodoTaskDto
        {
            Description = request.Description,
            Deadline = request.Deadline,
            Status = request.Status
        };

        var validationResult = await _validator.ValidateAsync(updateDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Task update validation failed: {Errors}", errors);
            throw new ValidationException(validationResult.Errors);
        }

        // Update only provided fields
        if (!string.IsNullOrEmpty(request.Description))
            task.Description = request.Description;
        
        if (request.Deadline.HasValue)
            task.Deadline = request.Deadline;
        
        if (request.Status.HasValue)
            task.Status = request.Status.Value;

        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task with ID {TaskId} updated successfully", request.Id);

        return _mapper.Map<TodoTaskDto>(task);
    }
}
