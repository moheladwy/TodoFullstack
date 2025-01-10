using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Helpers;
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
public class ListsController(
    IRepository<TaskList, AddListDto, UpdateListDto> listRepository,
    ILogger<ListsController> logger,
    IAccountRepository accountRepository,
    IValidator<AddListDto> addListValidator,
    IValidator<UpdateListDto> updateListValidator) : ControllerBase
{

    /// <summary>
    ///     Get all lists for the current user from the database.
    /// </summary>
    /// <returns>
    ///     A list of ItemsList objects for the current user which includes the items in each list in case they exist.
    ///     If no lists are found, an empty list is returned.
    ///     If an exception occurs, a BadRequest response is returned with the exception message.
    /// </returns>
    [HttpGet("all-lists")]
    public async Task<ActionResult<IEnumerable<ListsDto>>> GetAllLists()
    {
        var authenticatedUser = await accountRepository.GetUserByClaims(User);
        var lists = await listRepository.GetAllAsync(authenticatedUser.Id);

        logger.LogInformation("Lists for user with ID: {userId} retrieved successfully.",
         authenticatedUser.Id);
        return Ok(ListsHelper.MapToListsDto(lists));
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
        var list = await listRepository.GetByIdAsync(listId);

        logger.LogInformation("List with ID: {listId} retrieved successfully.", listId);
        return Ok(ListsHelper.MapToListsDto(list));
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
        var validationResult = await addListValidator.ValidateAsync(addListDto);
        if (!validationResult.IsValid)
            throw new InvalidModelStateException(validationResult.ToString());

        var authenticatedUser = await accountRepository.GetUserByClaims(User);
        var list = await listRepository.AddAsync(new AddListDto
        {
            Name = addListDto.Name,
            Description = addListDto.Description,
            UserId = authenticatedUser.Id
        });

        logger.LogInformation("New list with ID: {id} added successfully to the user with ID: {userId}.", list.Id,
            authenticatedUser.Id);
        return Ok(ListsHelper.MapToListsDto(list));
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
        var validationResult = await updateListValidator.ValidateAsync(list);
        if (!validationResult.IsValid)
            throw new InvalidModelStateException(validationResult.ToString());

        var updatedTask = await listRepository.UpdateAsync(list);

        logger.LogInformation("List with ID: {entity.Id} updated successfully.", list.Id);
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
        await listRepository.DeleteAsync(id);

        logger.LogInformation("List with ID: {id} and all its tasks deleted successfully.", id);
        return Ok();
    }
}