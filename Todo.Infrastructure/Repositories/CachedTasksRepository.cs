using Microsoft.Extensions.Caching.Memory;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;
using Task_Entity = Todo.Core.Entities.Task;

namespace Todo.Infrastructure.Repositories;

public class CachedTasksRepository : IRepository<Task_Entity, AddTaskDto, UpdateTaskDto>
{
    private readonly TasksRepository _tasksRepository;
    private readonly IMemoryCache _memoryCache;

    public CachedTasksRepository(TasksRepository tasksRepository, IMemoryCache memoryCache)
    {
        _tasksRepository = tasksRepository;
        _memoryCache = memoryCache;
    }
    
    public async Task<IEnumerable<Task_Entity>> GetAllAsync(string id)
    {
        var cacheKey = $"List-{id}-Tasks";

        var cachedTasks = await _memoryCache.GetOrCreateAsync<IEnumerable<Task_Entity>>(
            cacheKey,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching));
                return _tasksRepository.GetAllAsync(id);
            });

        return cachedTasks ?? [];
    }

    public async Task<Task_Entity> GetByIdAsync(Guid id)
    {
        var cacheKey = $"Task-{id}";

        var cachedTask = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching));
                return _tasksRepository.GetByIdAsync(id);
            });

        return cachedTask ?? throw new TaskNotFoundException($"The task with id: {id} was not found.");
    }

    public async Task<Task_Entity> AddAsync(AddTaskDto entity)
    {
        var addedTask = await _tasksRepository.AddAsync(entity);
        await UpdateAllTasksInCache(entity.ListId.ToString());
        return addedTask;
    }

    public async Task<Task_Entity> UpdateAsync(UpdateTaskDto entity)
    {
        var updatedTask = await _tasksRepository.UpdateAsync(entity);

        var taskEntity = await _tasksRepository.GetByIdAsync(entity.Id);
        await UpdateAllTasksInCache(taskEntity.ListId.ToString() ??
                                    throw new ArgumentNullException(taskEntity.ListId.ToString(), "The ListId cannot be null."));

        return updatedTask;
    }
    
    public Task_Entity UpdateEntity(Task_Entity entity, UpdateTaskDto dto) => _tasksRepository.UpdateEntity(entity, dto);

    public async Task DeleteAsync(Guid id)
    {
        var task = await GetByIdAsync(id);
        await _tasksRepository.DeleteAsync(id);

        _memoryCache.Remove($"Task-{id}");

        await UpdateAllTasksInCache(task.ListId.ToString() ??
                                    throw new ArgumentNullException(task.ListId.ToString(), "The ListId cannot be null."));
    }
    
    private async Task UpdateAllTasksInCache(string listId)
    {
        var cacheKey = $"List-{listId}-Tasks";
        _memoryCache.Remove(cacheKey);
        _memoryCache.CreateEntry(cacheKey).Value = await _tasksRepository.GetAllAsync(listId);
    }
}