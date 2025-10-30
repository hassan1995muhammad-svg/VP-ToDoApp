using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using VP_ToDo_App.Application.DTOs;
using VP_ToDo_App.Domain.Entities;
using VP_ToDo_App.Infrastructure.Repositories;

namespace VP_ToDo_App.Application.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TodoTaskDto>
{
    private readonly ITodoTaskRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateTodoTaskDto> _validator;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(
        ITodoTaskRepository repository,
        IMapper mapper,
        IValidator<CreateTodoTaskDto> validator,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task<TodoTaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new task with description: {Description}", request.Description);

        var createDto = new CreateTodoTaskDto
        {
            Description = request.Description,
            Deadline = request.Deadline
        };

        var validationResult = await _validator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
            _logger.LogWarning("Task creation validation failed: {Errors}", errors);
            throw new ValidationException(validationResult.Errors);
        }

        var task = _mapper.Map<TodoTask>(createDto);
        await _repository.AddAsync(task, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Task created successfully with ID: {TaskId}", task.Id);

        return _mapper.Map<TodoTaskDto>(task);
    }
}
