using API.DatabaseContexts;
using API.Exceptions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repository;

public class TasksRepository : IRepository<Models.Task>
{
    private readonly TodoIdentityContext _identityContext;
    private readonly ILogger<TasksRepository> _logger;

    public TasksRepository(TodoIdentityContext identityContext, ILogger<TasksRepository> logger)
    {
        _identityContext = identityContext;
        _logger = logger;
    }

    public async Task<IEnumerable<Models.Task>> GetAllAsync(string comparable)
    {
        return await _identityContext.Tasks
            .AsNoTracking()
            .Where(l => l.ListId == comparable)
            .ToListAsync();
    }

    public Task<Models.Task?> GetByIdAsync(string id)
    {
        return _identityContext.Tasks
            .AsNoTracking()
            .Where(l => l.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Models.Task> AddAsync(Models.Task entity)
    {
        _ = await _identityContext.Lists
                .Where(l => l.Id == entity.ListId)
                .FirstAsync() ??
            throw new ListNotFoundException($"List with the specified ID: {entity.ListId} not found.");
        _identityContext.Tasks.Add(entity); // Add the new Task to the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        _logger.LogInformation("Task with ID: {entity.Id} added to the list with ID: {entity.ListId}", entity.Id, entity.ListId);
        return entity;
    }

    public async Task<Models.Task> UpdateAsync(Models.Task entity)
    {
        _identityContext.Tasks.Update(entity); // Update the list in the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        _logger.LogInformation("Task with ID: {entity.Id} updated.", entity.Id);
        return entity;
    }

    public async Task DeleteAsync(string id)
    {
        var task = await _identityContext.Tasks
            .Where(l => l.Id == id)
            .FirstAsync() ??
            throw new TaskNotFoundException($"Task with the specified ID: {id} not found.");
        
        _identityContext.Tasks.Remove(task);
        await _identityContext.SaveChangesAsync();
        _logger.LogInformation("Task with ID: {id} deleted.", id);
    }
}