using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.DTOs.ListDTOs;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;

namespace Todo.Api.Controllers;

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
        _authenticatedUser = await _accountService.GetUserByClaims(User);
        var lists = await _listRepository.GetAllAsync(_authenticatedUser.Id);
        
        _logger.LogInformation("Lists for user with ID: {userId} retrieved successfully.", _authenticatedUser.Id);
        return Ok(lists);
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
        var list = await _listRepository.GetByIdAsync(listId);
        
        _logger.LogInformation("List with ID: {listId} retrieved successfully.", listId);
        return Ok(list);
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
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state because of the following errors: {ModelState.ValidationState}");

        _authenticatedUser = await _accountService.GetUserByClaims(User);
        if (_authenticatedUser.Id != addListDto.UserId)
            throw new UnauthorizedAccessException($"User with Id: {_authenticatedUser.Id} is not authorized to add a list for another user.");

        var list = await _listRepository.AddAsync(addListDto);

        _logger.LogInformation("New list with ID: {id} added successfully to the user with ID: {userId}.", list.Id,
            _authenticatedUser.Id);
        return Ok(list);
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
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state because of the following errors: {ModelState.ValidationState}");

        var updatedTask = await _listRepository.UpdateAsync(list);
        
        _logger.LogInformation("List with ID: {entity.Id} updated successfully.", list.Id);
        return Ok(updatedTask);
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
        await _listRepository.DeleteAsync(id);
        
        _logger.LogInformation("List with ID: {id} and all its tasks deleted successfully.", id);
        return Ok();
    }
}