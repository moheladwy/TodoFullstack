using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Repositories.DB;
using Task_Entity = Todo.Core.Entities.Task;

namespace Todo.Infrastructure.Repositories.Cached;

public class CachedTasksRepository : IRepository<Task_Entity, AddTaskDto, UpdateTaskDto>
{
    private readonly TasksRepository _tasksRepository;
    private readonly IRedisCacheService _cacheService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CachedTasksRepository"/> class.
    /// </summary>
    /// <param name="tasksRepository"></param>
    /// <param name="cacheService"></param>
    public CachedTasksRepository(TasksRepository tasksRepository, IRedisCacheService cacheService)
    {
        _tasksRepository = tasksRepository;
        _cacheService = cacheService;
    }
    
    public async Task<IEnumerable<Task_Entity>> GetAllAsync(string id)
    {
        var cacheKey = $"List-{id}-Tasks";

        var cachedTasks = await _cacheService.GetData<IEnumerable<Task_Entity>>(cacheKey);
        if (cachedTasks is not null) return cachedTasks;

        var tasks = await _tasksRepository.GetAllAsync(id);
        var taskEntities = tasks.ToList();
        await _cacheService.SetData(cacheKey, taskEntities);
        return taskEntities;
    }

    public async Task<Task_Entity> GetByIdAsync(Guid id)
    {
        var cacheKey = $"Task-{id}";

        var cachedTask = await _cacheService.GetData<Task_Entity>(cacheKey);
        if (cachedTask is not null) return cachedTask;

        var task = await _tasksRepository.GetByIdAsync(id);
        await _cacheService.SetData(cacheKey, task);
        return task;
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
        await _cacheService.RemoveData($"Task-{id}");
        await UpdateAllTasksInCache(task.ListId.ToString() ??
                                    throw new ArgumentNullException(task.ListId.ToString(), "The ListId cannot be null."));
    }
    
    private async Task UpdateAllTasksInCache(string listId)
    {
        var cacheKey = $"List-{listId}-Tasks";
        var task = await _tasksRepository.GetAllAsync(listId);
        var taskEntities = task.ToList();
        await _cacheService.RemoveData(cacheKey);
        await _cacheService.SetData(cacheKey, taskEntities);
    }
}