using MediatR;
using Microsoft.AspNetCore.Mvc;
using VP_ToDo_App.Application.Commands.CreateTask;
using VP_ToDo_App.Application.Commands.DeleteTask;
using VP_ToDo_App.Application.Commands.MarkTaskAsCompleted;
using VP_ToDo_App.Application.Commands.UpdateTask;
using VP_ToDo_App.Application.Queries.GetAllTasks;
using VP_ToDo_App.Application.Queries.GetTaskById;
using VP_ToDo_App.Application.Queries.GetTasksByStatus;
using VP_ToDo_App.Application.Queries.SearchTasks;
using VP_ToDo_App.Domain.Enums;

namespace VP_ToDo_App.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(IMediator mediator, ILogger<TasksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTasks(CancellationToken cancellationToken)
    {
        var query = new GetAllTasksQuery();
        var tasks = await _mediator.Send(query, cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Get task by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTaskById(int id, CancellationToken cancellationToken)
    {
        var query = new GetTaskByIdQuery { Id = id };
        var task = await _mediator.Send(query, cancellationToken);
        
        if (task == null)
            return NotFound(new { message = $"Task with ID {id} not found." });

        return Ok(task);
    }

    /// <summary>
    /// Get tasks by status (Active or Completed)
    /// </summary>
    [HttpGet("status/{status}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTasksByStatus(Domain.Enums.TaskStatus status, CancellationToken cancellationToken)
    {
        var query = new GetTasksByStatusQuery { Status = status };
        var tasks = await _mediator.Send(query, cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Search tasks by description
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchTasks([FromQuery] string query, CancellationToken cancellationToken)
    {
        var searchQuery = new SearchTasksQuery { SearchQuery = query ?? string.Empty };
        var tasks = await _mediator.Send(searchQuery, cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var task = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;
        var task = await _mediator.Send(command, cancellationToken);
        return Ok(task);
    }

    /// <summary>
    /// Mark task as completed
    /// </summary>
    [HttpPatch("{id}/complete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkTaskAsCompleted(int id, CancellationToken cancellationToken)
    {
        var command = new MarkTaskAsCompletedCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new { message = "Task marked as completed successfully." });
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteTaskCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(new { message = "Task deleted successfully." });
    }
}
