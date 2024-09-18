using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using API.DTOs.ListsDTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers;

/// <summary>
///     Controller for managing lists of items.
///     Requires the user to be authenticated.
///     It has the following endpoints:
/// </summary>
/// <code>
///     - GET /api/lists/all-lists/{userId}
///     - GET /api/lists/get-list/{id}
///     - POST /api/lists/add-list
///     - PUT /api/lists/update-list
///     - DELETE /api/lists/delete-list/{id}
/// </code>
[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = Roles.User, Policy = Roles.User)]
public class ListsController : ControllerBase // BUG: The signed user cannot access the controller at all because of Authorization.
{
    private readonly IRepository<TaskList> _listRepository;
    private readonly ILogger<ListsController> _logger;
    private readonly User _user;

    /// <summary>
    ///     Constructor for ListsController class.
    /// </summary>
    /// <param name="listRepository">
    ///     The repository for managing lists of items.
    /// </param>
    /// <param name="logger">
    ///     The logger for the ListsController class.
    /// </param>
    /// <param name="principal">
    ///     The current user principal that is making the request.
    /// </param>
    /// <param name="signInManager">
    ///     The sign-in manager for the user.
    /// </param>
    /// <exception cref="UnauthorizedAccessException">
    ///     Thrown when the user is not authenticated or not found.
    /// </exception>
    public ListsController(
        IRepository<TaskList> listRepository,
        ILogger<ListsController> logger,
        ClaimsPrincipal principal,
        SignInManager<User> signInManager)
    {
        _listRepository = listRepository;
        _logger = logger;
        if (signInManager.IsSignedIn(principal))
        {
            _logger.LogError("User is not authenticated.");
            throw new UnauthorizedAccessException("User is not authenticated to in the system.");
        }

        _user  = signInManager.UserManager.GetUserAsync(principal).Result ?? throw new UnauthorizedAccessException("Unauthorized access for the user.");
    }

    /// <summary>
    ///     Get all lists for the current user from the database.
    /// </summary>
    /// <returns>
    ///     A list of ItemsList objects for the current user which includes the items in each list in case they exist.
    ///     If no lists are found, an empty list is returned.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpGet("all-lists")]
    public async Task<ActionResult<List<TaskList>>> GetAllLists()
    {
        try
        {
            var lists = await _listRepository.GetAllAsync(_user.Id);
            var listsCount = lists.ToList().Count;
            _logger.LogInformation(listsCount == 0 ? "No lists found for user with ID: {userId}." : "Lists for user with ID: {userId} retrieved successfully.", _user.Id);
            return Ok(lists);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving lists for user with ID: {Id}, because {error}.", _user.Id, e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Get a list by its ID for the current user from the database.
    /// </summary>
    /// <param name="listId">
    ///     The ID of the list to get.
    /// </param>
    /// <returns>
    ///     The ItemsList object with the specified ID for the current user which includes the items in the list in case they exist.
    ///     If the list is not found, a NotFound response is returned.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpGet("get-list/{listId}")]
    public async Task<ActionResult<TaskList>> GetList([FromRoute] string listId)
    {
        try
        {
            var list = await _listRepository.GetByIdAsync(listId);
            _logger.LogInformation(list != null ? "List with ID: {listId} retrieved successfully." : "List with ID: {listId} not found.", listId);
            return list == null ? NotFound($"List with the specified ID: {listId} not found.") : Ok(list);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving list with ID: {listId}, because {error}.", listId, e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Add a new list to the database for the current user.
    /// </summary>
    /// <param name="addListDto">
    ///     The AddListDto object with the name and description of the new list.
    /// </param>
    /// <returns>
    ///     If everything is ok it returns the newly created TaskList object with the specified name, description,
    ///     the current user as the owner, and the new generated ID.
    ///     If the model state is not valid, a BadRequest response is returned with the model state.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpPost("add-list")]
    public async Task<ActionResult<TaskList>> AddList([FromBody] AddListDto addListDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for adding new list.");
                return BadRequest(ModelState);
            }

            var newList = await _listRepository.AddAsync(new TaskList()
            {
                Name = addListDto.Name,
                Description = addListDto.Description ?? string.Empty,
                User = _user
            });

            _logger.LogInformation("New list with ID: {id} added successfully.", newList.Id);
            return Ok(newList);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error adding new list with name: {name}, because {error}.", addListDto.Name, e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Update an existing list in the database for the current user.
    /// </summary>
    /// <param name="list">
    ///     The TaskList object with the new values for the list.
    /// </param>
    /// <returns>
    ///     If everything is ok it returns the updated TaskList object with the new values.
    ///     If the model state is not valid, a BadRequest response is returned with the model state.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpPut("update-list")]
    public async Task<ActionResult<TaskList>> UpdateList([FromBody] TaskList list)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for updating list.");
                return BadRequest(ModelState);
            }

            var updatedTask = await _listRepository.UpdateAsync(list);
            _logger.LogInformation("List with ID: {id} updated successfully.", updatedTask.Id);

            return Ok(updatedTask);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating list with ID: {id}, because {error}.", list.Id, e.Message);
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Delete a list by its ID for the current user from the database.
    /// </summary>
    /// <param name="id">
    ///     The ID of the list to delete.
    /// </param>
    /// <returns>
    ///     If everything is ok it returns an Ok response.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpDelete("delete-list/{id}")]
    public async Task<ActionResult> DeleteList([FromRoute] string id)
    {
        try
        {
            await _listRepository.DeleteAsync(id);
            _logger.LogInformation("List with ID: {id} deleted successfully.", id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting list with ID: {id}, because {error}.", id, e.Message);
            return BadRequest(e.Message);
        }
    }
}

