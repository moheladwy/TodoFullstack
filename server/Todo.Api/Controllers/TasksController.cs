using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Entity_Task = Todo.Core.Entities.Task;

namespace Todo.Api.Controllers;

/// <summary>
///     Controller for tasks.
/// </summary>
/// <code>
///     - GET    api/tasks/all-tasks/{listId}
///     - GET    api/tasks/get-task/{id}
///     - POST   api/tasks/add-task
///     - PUT    api/tasks/update-task
///     - DELETE api/tasks/delete-task/{id}
/// </code>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TasksController : ControllerBase
{
    private readonly IRepository<Entity_Task, AddTaskDto, UpdateTaskDto> _taskRepository;
    private readonly ILogger<TasksController> _logger;

    /// <summary>
    ///     Constructor for the TasksController.
    /// </summary>
    /// <param name="taskRepository">
    ///     The repository for tasks.
    /// </param>
    /// <param name="logger">
    ///     The logger for the TasksController.
    /// </param>
    public TasksController(IRepository<Entity_Task, AddTaskDto, UpdateTaskDto> taskRepository,
        ILogger<TasksController> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    /// <summary>
    ///     Get all tasks for a specific list by listId.
    /// </summary>
    /// <param name="listId">
    ///     The id of the list to get all tasks for.
    /// </param>
    /// <returns>
    ///     A list of tasks for the specified list.
    ///     If no tasks are found, an empty list is returned.
    ///     If an error occurs, a bad request is returned with the error message.
    /// </returns>
    [HttpGet("all-tasks/{listId}")]
    public async Task<ActionResult<List<Entity_Task>>> GetAllTasksByListId([FromRoute] string listId)
    {
        var tasks = await _taskRepository.GetAllAsync(listId);

        _logger.LogInformation("Tasks found for list with id: {listId}", listId);
        return Ok(tasks);
    }

    /// <summary>
    ///     Get a specific task by id.
    /// </summary>
    /// <param name="id">
    ///     The id of the task to get.
    /// </param>
    /// <returns>
    ///     The task with the specified id.
    ///     If no task is found, a not found is returned.
    ///     If an error occurs, a bad request is returned with the error message.
    /// </returns>
    [HttpGet("get-task/{id}")]
    public async Task<ActionResult<Entity_Task>> GetTaskById(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);

        _logger.LogInformation("Task with ID: {taskId} retrieved successfully.", id);
        return Ok(task);
    }

    /// <summary>
    ///     Add a new task to the database with the specified listId.
    /// </summary>
    /// <param name="addTaskDto">
    ///     The task to add to the database.
    /// </param>
    /// <returns>
    ///     The task that was added to the database.
    ///     If an error occurs, a bad request is returned with the error message.
    /// </returns>
    [HttpPost("add-task")]
    public async Task<ActionResult<Entity_Task>> AddTask([FromBody] AddTaskDto addTaskDto)
    {
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state for adding a task with the error {ModelState.ValidationState}");

        var task = await _taskRepository.AddAsync(addTaskDto);

        _logger.LogInformation("Task with ID: {taskId} added successfully.", task.Id);
        return Ok(task);
    }

    /// <summary>
    ///     Update a task in the database.
    /// </summary>
    /// <param name="updateTaskDto">
    ///     The task to be updated in the database.
    /// </param>
    /// <returns>
    ///     The updated task.
    /// </returns>
    [HttpPut("update-task")]
    public async Task<ActionResult<Entity_Task>> UpdateTask([FromBody] UpdateTaskDto updateTaskDto)
    {
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state for updating a task with the error {ModelState.ValidationState}");
        
        var task = await _taskRepository.UpdateAsync(updateTaskDto);
        
        _logger.LogInformation("Task with ID: {taskId} updated successfully.", task.Id);
        return Ok(task);
    }

    /// <summary>
    ///     Delete a task from the database by id.
    /// </summary>
    /// <param name="id">
    ///     The id of the task to be deleted.
    /// </param>
    /// <returns>
    ///     An OK response if the task was deleted successfully.
    ///     If the task was not found, a not found response is returned.
    ///     If an error occurs, a bad request is returned with the error message.
    /// </returns>
    [HttpDelete("delete-task/{id}")]
    public async Task<ActionResult> DeleteTask([FromRoute] Guid id)
    {
        await _taskRepository.DeleteAsync(id);
        
        _logger.LogInformation("Task with ID: {taskId} deleted successfully.", id);
        return Ok();
    }
}