using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models_Task = API.Models.Task;
using Task = API.Models.Task;

namespace API.Controllers;

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
[Authorize(Roles.User)]
public class TasksController : ControllerBase
{
    private readonly IRepository<Models_Task> _taskRepository;

    /// <summary>
    ///     Constructor for the TasksController.
    /// </summary>
    /// <param name="taskRepository">
    ///     The repository for tasks.
    /// </param>
    public TasksController(IRepository<Models_Task> taskRepository) => _taskRepository = taskRepository;

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
    public async Task<ActionResult<List<Models_Task>>> GetAllItems([FromRoute] string listId)
    {
        try
        {
            var tasks = await _taskRepository.GetAllAsync(listId);
            return Ok(tasks);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
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
    public async Task<ActionResult<Models_Task>> GetItem(string id)
    {
        try
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task == null ? NotFound() : Ok(task);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }

    /// <summary>
    ///     Add a new task to the database with the specified listId.
    /// </summary>
    /// <param name="task">
    ///     The task to add to the database.
    /// </param>
    /// <returns>
    ///     The task that was added to the database.
    ///     If an error occurs, a bad request is returned with the error message.
    /// </returns>
    [HttpPost("add-task")]
    public async Task<ActionResult<Models_Task>> AddItem([FromBody] Models_Task task)
    {
        try
        {
            task = await _taskRepository.AddAsync(task);
            return Ok(task);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }

    /// <summary>
    ///     Update a task in the database.
    /// </summary>
    /// <param name="task">
    ///     The task to be updated in the database.
    /// </param>
    /// <returns>
    ///     The updated task.
    /// </returns>
    [HttpPut("update-task")]
    public async Task<ActionResult<Models_Task>> UpdateItem(Models_Task task)
    {
        try
        {
            task = await _taskRepository.UpdateAsync(task);
            return Ok(task);
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
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
    public async Task<ActionResult> DeleteItem(string id)
    {
        try
        {
            await _taskRepository.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest("Error: " + e.Message);
        }
    }
}
