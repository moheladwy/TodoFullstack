using api.DTOs.ListDTOs;
using Microsoft.AspNetCore.Mvc;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

/// <summary>
///     Controller for managing lists of items.
///     Requires the user to be authenticated.
///     It has the following endpoints:
/// </summary>
/// <code>
///     - GET /api/lists/all-lists
///     - GET /api/lists/get-list/{id}
///     - POST /api/lists/add-list
///     - PUT /api/lists/update-list
///     - DELETE /api/lists/delete-list/{id}
/// </code>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ListsController : ControllerBase
{
    private readonly IRepository<TaskList, AddListDto, UpdateListDto> _listRepository;
    private readonly ILogger<ListsController> _logger;
    private readonly IAccountService _accountService;
    private User? _authenticatedUser;

    /// <summary>
    ///     Constructor for ListsController class.
    /// </summary>
    /// <param name="listRepository">
    ///     The repository for managing lists of items.
    /// </param>
    /// <param name="logger">
    ///     The logger for the ListsController class.
    /// </param>
    /// <param name="accountService">
    ///     The account service for managing user operations.
    /// </param>
    public ListsController(
        IRepository<TaskList, AddListDto, UpdateListDto> listRepository,
        ILogger<ListsController> logger,
        IAccountService accountService)
    {
        _listRepository = listRepository;
        _accountService = accountService;
        _logger = logger;
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
            _authenticatedUser = await _accountService.GetUserByClaims(User);
            var lists = await _listRepository.GetAllAsync(_authenticatedUser.Id);
            _logger.LogInformation("Lists for user with ID: {userId} retrieved successfully.", _authenticatedUser.Id);
            return Ok(lists);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error retrieving lists for user with ID: {Id}, because {error}.", _authenticatedUser.Id, e.Message);
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
    ///     The TaskList object with the specified ID for the current user which includes the items in the list in case they exist.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpGet("get-list/{listId}")]
    public async Task<ActionResult<TaskList>> GetListById([FromRoute] Guid listId)
    {
        try
        {
            var list = await _listRepository.GetByIdAsync(listId);
            _logger.LogInformation("List with ID: {listId} retrieved successfully." , listId);
            return Ok(list);
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

            _authenticatedUser = await _accountService.GetUserByClaims(User);
            if (_authenticatedUser.Id != addListDto.UserId)
            {
                _logger.LogError("User with ID: {userId} is not authorized to add a list for user with ID: {addListDto.UserId}.", _authenticatedUser.Id, addListDto.UserId);
                return BadRequest("You are not authorized to add a list for another user.");
            }

            var list = await _listRepository.AddAsync(addListDto);

            _logger.LogInformation("New list with ID: {id} added successfully to the user with ID: {userId}.", list.Id, _authenticatedUser.Id);
            return Ok(list);
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
    public async Task<ActionResult<TaskList>> UpdateList([FromBody] UpdateListDto list)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for updating list.");
                return BadRequest(ModelState);
            }

            var updatedTask = await _listRepository.UpdateAsync(list);
            _logger.LogInformation("List with ID: {entity.Id} updated successfully.", list.Id);
            return Ok(updatedTask);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating list with ID: {id}, because {error}.", list.Id, e.Message);
            return BadRequest($"Error updating list with ID: {list.Id}, because {e.Message}.");
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
    public async Task<ActionResult> DeleteList([FromRoute] Guid id)
    {
        try
        {
            await _listRepository.DeleteAsync(id);
            _logger.LogInformation("List with ID: {id} and all its tasks deleted successfully.", id);
            return Ok();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting list with ID: {id}, because {error}.", id, e.Message);
            return BadRequest(e.Message);
        }
    }
}

