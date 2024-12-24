using Todo.Core.DTOs.ListDTOs;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Repositories.DB;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Repositories.Cached;

public class CachedListRepository : IRepository<TaskList, AddListDto, UpdateListDto>
{
    private readonly ListRepository _listRepository;
    private readonly IRedisCacheService _cacheService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CachedListRepository" /> class.
    /// </summary>
    /// <param name="listRepository"></param>
    /// <param name="cacheService"></param>
    public CachedListRepository(ListRepository listRepository, IRedisCacheService cacheService)
    {
        _listRepository = listRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<TaskList>> GetAllAsync(string id)
    {
        var cacheKey = $"User-{id}-Lists";

        var cachedLists = await _cacheService.GetData<IEnumerable<TaskList>>(cacheKey);
        if (cachedLists is not null) return cachedLists;

        var lists = await _listRepository.GetAllAsync(id);
        var taskLists = lists.ToList();
        await _cacheService.SetData(cacheKey, taskLists);
        return taskLists;
    }

    public async Task<TaskList> GetByIdAsync(Guid id)
    {
        var cacheKey = $"List-{id}";

        var cachedList = await _cacheService.GetData<TaskList>(cacheKey);
        if (cachedList is not null) return cachedList;

        var list = await _listRepository.GetByIdAsync(id);
        await _cacheService.SetData(cacheKey, list);
        return list;
    }

    public async Task<TaskList> AddAsync(AddListDto entity)
    {
        var addedList = await _listRepository.AddAsync(entity);
        await UpdateAllListsInCache(entity.UserId);
        return addedList;
    }

    public async Task<TaskList> UpdateAsync(UpdateListDto entity)
    {
        var updatedList = await _listRepository.UpdateAsync(entity);

        var cacheKey = $"List-{entity.Id}";
        await _cacheService.UpdateData(cacheKey, updatedList);

        var list = await _listRepository.GetByIdAsync(entity.Id);
        await UpdateAllListsInCache(list.UserId ?? throw new ArgumentNullException(list.UserId,
            "The list with the given id does not have a user id."));

        return updatedList;
    }

    public TaskList UpdateEntity(TaskList entity, UpdateListDto dto) => _listRepository.UpdateEntity(entity, dto);

    public async Task DeleteAsync(Guid id)
    {
        var list = await _listRepository.GetByIdAsync(id);
        await _listRepository.DeleteAsync(id);

        await _cacheService.RemoveData($"List-{id}");
        await UpdateAllListsInCache(list.UserId ?? throw new ArgumentNullException(list.UserId,
            "The list with the given id does not have a user id."));
    }

    private async Task UpdateAllListsInCache(string userId)
    {
        var cacheKey = $"User-{userId}-Lists";
        var lists = await _listRepository.GetAllAsync(userId);
        var taskLists = lists.ToList();
        await _cacheService.UpdateData(cacheKey, taskLists);
    }
}