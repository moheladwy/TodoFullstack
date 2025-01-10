using FluentValidation;
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
public class TasksController(IRepository<Entity_Task, AddTaskDto, UpdateTaskDto> taskRepository,
    ILogger<TasksController> logger,
    IValidator<AddTaskDto> addTaskValidator,
    IValidator<UpdateTaskDto> updateTaskValidator) : ControllerBase
{

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
        if (string.IsNullOrEmpty(listId))
            throw new InvalidModelStateException("List ID cannot be null or empty.");
        var tasks = await taskRepository.GetAllAsync(listId);

        logger.LogInformation("Tasks found for list with id: {listId}", listId);
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
        if (id == Guid.Empty)
            throw new InvalidModelStateException("Task ID cannot be null or empty.");
        var task = await taskRepository.GetByIdAsync(id);

        logger.LogInformation("Task with ID: {taskId} retrieved successfully.", id);
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
        var validationResult = addTaskValidator.Validate(addTaskDto);
        if (!validationResult.IsValid)
            throw new InvalidModelStateException(validationResult.ToString());

        var task = await taskRepository.AddAsync(addTaskDto);

        logger.LogInformation("Task with ID: {taskId} added successfully.", task.Id);
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
        var validationResult = updateTaskValidator.Validate(updateTaskDto);
        if (!validationResult.IsValid)
            throw new InvalidModelStateException(validationResult.ToString());

        var task = await taskRepository.UpdateAsync(updateTaskDto);

        logger.LogInformation("Task with ID: {taskId} updated successfully.", task.Id);
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
        if (id == Guid.Empty)
            throw new InvalidModelStateException("Task ID cannot be null or empty.");
        await taskRepository.DeleteAsync(id);

        logger.LogInformation("Task with ID: {taskId} deleted successfully.", id);
        return Ok();
    }
}