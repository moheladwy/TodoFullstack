using Todo.Api.Data.DatabaseContexts;
using Todo.Api.Models.DTOs.ListDTOs;
using Todo.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Todo.Api.Exceptions;
using Todo.Api.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Todo.Api.Repository;

/// <summary>
///     A repository for managing the task lists.
/// </summary>
public class ListRepository : IRepository<TaskList, AddListDto, UpdateListDto>
{
    /// <summary>
    ///     The database context for the task lists.
    /// </summary>
    private readonly TodoIdentityContext _identityContext;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ListRepository"/> class.
    /// </summary>
    /// <param name="identityContext">
    ///     The database context for the task lists.
    /// </param>
    public ListRepository(TodoIdentityContext identityContext) => _identityContext = identityContext;

    /// <summary>
    ///     Gets all the task lists for the specified user.
    /// </summary>
    /// <param name="id">
    ///     The ID of the user to fetch the lists for.
    /// </param>
    /// <returns>
    ///     A collection of task lists for the specified user.
    /// </returns>
    public async Task<IEnumerable<TaskList>> GetAllAsync(string id)
    {
        return await _identityContext.Lists
            .AsNoTracking() // No need to track the entities since we are not going to modify them in this request (we are only reading them).
            .Where(l => l.UserId == id) // Filter the lists by the current user.
            .Include(l => l.Tasks) // Include the items in the list.
            .ToListAsync(); // Execute the query and return the results as a list.
    }

    /// <summary>
    ///     Gets a task list by its ID.
    /// </summary>
    /// <param name="id">
    ///     The ID of the task list to fetch.
    /// </param>
    /// <returns>
    ///     The task list with the specified ID.
    /// </returns>
    public async Task<TaskList> GetByIdAsync(Guid id)
    {
        return await _identityContext.Lists
            .AsNoTracking() // No need to track the entities since we are not going to modify them in this request (we are only reading them).
            .Where(l => l.Id == id) // Filter the lists by the specified ID.
            .Include(l => l.Tasks) // Include the items in the list.
            .FirstAsync(); // Execute the query and return the first result.
    }

    /// <summary>
    ///     Adds a new task list to the database.
    /// </summary>
    /// <param name="entity">
    ///     The task list to add to the database.
    /// </param>
    /// <returns>
    ///     The task list that was added to the database.
    /// </returns>
    public async Task<TaskList> AddAsync(AddListDto entity)
    {
        var list = new TaskList
        {
            Name = entity.Name,
            Description = entity.Description ?? string.Empty,
            UserId = entity.UserId
        };
        _identityContext.Lists.Add(list); // Add the new list to the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        return list;
    }

    /// <summary>
    ///     Updates a task list in the database.
    /// </summary>
    /// <param name="entity">
    ///     The task list to update in the database.
    /// </param>
    /// <returns>
    ///     The task list that was updated in the database.
    /// </returns>
    /// <exception cref="ListNotFoundException">
    ///     Thrown when the list with the specified ID is not found.
    /// </exception>
    public async Task<TaskList> UpdateAsync(UpdateListDto entity)
    {
        var list = await _identityContext.Lists
                       .Where(l => l.Id == entity.Id) // Filter the lists by the specified ID.
                       .FirstAsync() ?? // Fetch the entity to be updated.
                   throw new ListNotFoundException($"List with the specified ID: {entity.Id} not found.");

        list = UpdateEntity(list, entity); // Update the list entity with the new data.

        _identityContext.Lists.Update(list); // Update the list in the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.

        return list;
    }

    /// <summary>
    ///     Updates the task list entity with the new data.
    /// </summary>
    /// <param name="entity">
    ///     The task list entity to update.
    /// </param>
    /// <param name="dto">
    ///     The DTO that contains the new data for the task list.
    /// </param>
    /// <returns>
    ///     The updated task list entity.
    /// </returns>
    public TaskList UpdateEntity(TaskList entity, UpdateListDto dto)
    {
        entity.Name = dto.Name?.Length > 0 ? dto.Name : entity.Name;
        entity.Description = dto.Description ?? entity.Description;
        return entity;
    }

    /// <summary>
    ///     Deletes a task list from the database.
    /// </summary>
    /// <param name="id">
    ///     The ID of the task list to delete.
    /// </param>
    /// <exception cref="ListNotFoundException">
    ///     Thrown when the list with the specified ID is not found.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        var list = await _identityContext.Lists
                       .Where(l => l.Id == id) // Filter by the specified ID.
                       .Include(taskList => taskList.Tasks) // Include related items.
                       .FirstAsync() ?? // Fetch the entities to be removed.
                   throw new ListNotFoundException($"List with the specified ID: {id} not found.");

        _identityContext.Tasks.RemoveRange(list.Tasks); // Remove the tasks from the database context.
        _identityContext.Lists.Remove(list); // Remove the lists from the database context.

        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
    }
}