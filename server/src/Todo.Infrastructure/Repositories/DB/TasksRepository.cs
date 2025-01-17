using Microsoft.EntityFrameworkCore;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Enums;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Todo.Infrastructure.DatabaseContexts;
using Task_Entity = Todo.Core.Entities.Task;

namespace Todo.Infrastructure.Repositories.DB;

/// <summary>
///     A class that implements the IRepository interface for the Task entity.
/// </summary>
public class TasksRepository : IRepository<Task_Entity, AddTaskDto, UpdateTaskDto>
{
    /// <summary>
    ///     The database context for the Identity database.
    /// </summary>
    private readonly TodoIdentityContext _identityContext;

    /// <summary>
    ///     Initializes a new instance of the TasksRepository class.
    /// </summary>
    /// <param name="identityContext">
    ///     The database context for the Identity database.
    /// </param>
    public TasksRepository(TodoIdentityContext identityContext) => _identityContext = identityContext;

    /// <summary>
    ///     Gets all the Tasks from the database that belong to the specified List.
    /// </summary>
    /// <param name="id">
    ///     The ID of the List to get the Tasks from.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task contains an IEnumerable of Tasks.
    /// </returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when the specified ID is not a valid GUID.
    /// </exception>
    public async Task<IEnumerable<Task_Entity>> GetAllAsync(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            throw new ArgumentException("The specified ID is not a valid GUID.");

        return await _identityContext.Tasks
            .AsNoTracking()
            .Where(l => l.ListId == guidId)
            .ToListAsync();
    }

    /// <summary>
    ///     Gets a Task from the database with the specified ID.
    /// </summary>
    /// <param name="id">
    ///     The ID of the Task to get.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task contains a Model Task.
    /// </returns>
    public Task<Task_Entity> GetByIdAsync(Guid id)
    {
        return _identityContext.Tasks
            .AsNoTracking()
            .Where(l => l.Id == id)
            .FirstAsync();
    }

    /// <summary>
    ///     Adds a new Task to the database.
    /// </summary>
    /// <param name="dto">
    ///     The DTO that contains the data to create the new Task.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task contains the new Task.
    /// </returns>
    /// <exception cref="ListNotFoundException">
    ///     Thrown when the List with the specified ID is not found.
    /// </exception>
    public async Task<Task_Entity> AddAsync(AddTaskDto dto)
    {
        _ = await _identityContext.Lists
                .Where(l => l.Id == dto.ListId)
                .FirstAsync() ??
            throw new ListNotFoundException($"List with the specified ID: {dto.ListId} not found.");

        var task = new Task_Entity()
        {
            Name = dto.Name,
            Description = dto.Description ?? string.Empty,
            // DueDate = dto.DueDate ?? null,
            Priority = (TaskPriority)dto.Priority,
            ListId = dto.ListId
        };

        _identityContext.Tasks.Add(task); // Add the new Task to the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        return task;
    }

    /// <summary>
    ///     Updates a Task in the database.
    /// </summary>
    /// <param name="entity">
    ///     The DTO that contains the data to update the Task.
    /// </param>
    /// <returns>
    ///     A Task that represents the asynchronous operation. The Task contains the updated Task.
    /// </returns>
    /// <exception cref="TaskNotFoundException">
    ///     Thrown when the Task with the specified ID is not found.
    /// </exception>
    public async Task<Task_Entity> UpdateAsync(UpdateTaskDto entity)
    {
        var task = await _identityContext.Tasks
                       .Where(l => l.Id == entity.Id)
                       .FirstAsync() ??
                   throw new TaskNotFoundException($"Task with the specified ID: {entity.Id} not found.");

        task = UpdateEntity(task, entity); // Update the Task entity with the new data.

        _identityContext.Tasks.Update(task); // Update the list in the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        return task;
    }

    /// <summary>
    ///     Updates the Task entity with the new data from the DTO.
    /// </summary>
    /// <param name="entity">
    ///     The Task entity to update.
    /// </param>
    /// <param name="dto">
    ///     The DTO that contains the new data for the Task entity.
    /// </param>
    /// <returns>
    ///     The updated Task entity.
    /// </returns>
    public Task_Entity UpdateEntity(Task_Entity entity, UpdateTaskDto dto)
    {
        // Update the task's name if the new value is not null or empty.
        if (!string.IsNullOrEmpty(dto.Name))
            entity.Name = dto.Name;
        entity.Description = (dto.Description is not null) ? dto.Description : string.Empty;
        entity.IsCompleted = dto.IsCompleted;
        // Update the task's due date.
        // entity.DueDate = dto.DueDate;
        entity.Priority = dto.Priority;

        return entity;
    }

    /// <summary>
    ///     Deletes a Task from the database with the specified ID.
    /// </summary>
    /// <param name="id">
    ///     The ID of the Task to delete.
    /// </param>
    /// <exception cref="TaskNotFoundException">
    ///     Thrown when the Task with the specified ID is not found.
    /// </exception>
    public async Task DeleteAsync(Guid id)
    {
        var task = await _identityContext.Tasks
                       .Where(l => l.Id == id)
                       .FirstAsync() ??
                   throw new TaskNotFoundException($"Task with the specified ID: {id} not found.");

        _identityContext.Tasks.Remove(task);
        await _identityContext.SaveChangesAsync();
    }
}