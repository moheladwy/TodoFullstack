using API.DatabaseContexts;
using API.Exceptions;
using API.Interfaces;
using API.Models;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repository;

public class ListRepository : IRepository<TaskList>
{
    private readonly TodoIdentityContext _identityContext;
    private readonly ILogger<ListRepository> _logger;

    public ListRepository(TodoIdentityContext identityContext, ILogger<ListRepository> logger)
    {
        _identityContext = identityContext;
        _logger = logger;
    }

    public async Task<IEnumerable<TaskList>> GetAllAsync(string comparable)
    {
        return await _identityContext.Lists
            .AsNoTracking() // No need to track the entities since we are not going to modify them in this request (we are only reading them).
            .Where(l => l.User.Id == comparable) // Filter the lists by the current user.
            .Include(l => l.Tasks)  // Include the items in the list.
            .ToListAsync(); // Execute the query and return the results as a list.
    }

    public async Task<TaskList?> GetByIdAsync(string id)
    {
        return await _identityContext.Lists
            .AsNoTracking() // No need to track the entities since we are not going to modify them in this request (we are only reading them).
            .Where(l => l.Id == id) // Filter the lists by the current user and the specified ID.
            .Include(l => l.Tasks) // Include the items in the list.
            .FirstOrDefaultAsync(); // Execute the query and return the first result.
    }

    public async Task<TaskList> AddAsync(TaskList entity)
    {
        _identityContext.Lists.Add(entity); // Add the new list to the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        _logger.LogInformation($"List with ID: {entity.Id} added to the user with ID: {entity.User?.Id}");
        return entity;
    }

    public async Task<TaskList> UpdateAsync(TaskList entity)
    {
        _identityContext.Lists.Update(entity); // Update the list in the database context.
        await _identityContext.SaveChangesAsync(); // Save the changes to the database.
        _logger.LogInformation($"List with ID: {entity.Id} has been updated.");
        return entity;
    }

    public async Task DeleteAsync(string id)
    {
        var list = await _identityContext.Lists
            .Where(l => l.Id == id)                 // Filter by the specified ID.
            .Include(taskList => taskList.Tasks)    // Include related items.
            .FirstAsync() ??                        // Fetch the entities to be removed.
            throw new ListNotFoundException($"List with the specified ID: {id} not found.");

        _identityContext.Tasks.RemoveRange(list.Tasks); // Remove the tasks from the database context.
        _identityContext.Lists.Remove(list);            // Remove the lists from the database context.

        await _identityContext.SaveChangesAsync();      // Save the changes to the database.
        _logger.LogInformation($"List with ID: {list.Id} and its tasks have been deleted successfully.");
    }
}