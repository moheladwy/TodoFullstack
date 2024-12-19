using Microsoft.Extensions.Caching.Memory;
using Todo.Core.DTOs.ListDTOs;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Repositories;

public class CachedListRepository : IRepository<TaskList, AddListDto, UpdateListDto>
{
    private readonly ListRepository _listRepository;
    private readonly IMemoryCache _memoryCache;

    public CachedListRepository(ListRepository listRepository, IMemoryCache memoryCache)
    {
        _listRepository = listRepository;
        _memoryCache = memoryCache;
    }

    public async Task<IEnumerable<TaskList>> GetAllAsync(string id)
    {
        var cacheKey = $"User-{id}-Lists";

        var cachedLists = await _memoryCache.GetOrCreateAsync<IEnumerable<TaskList>>(
            cacheKey,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching));
                return _listRepository.GetAllAsync(id);
            });

        return cachedLists ?? [];
    }

    public async Task<TaskList> GetByIdAsync(Guid id)
    {
        var cacheKey = $"List-{id}";

        var cachedList = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching));
                return _listRepository.GetByIdAsync(id);
            });

        return cachedList ?? throw new ListNotFoundException($"The list with id: {id} was not found.");
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
        _memoryCache.Remove(cacheKey);
        _memoryCache.CreateEntry(cacheKey).Value = updatedList;

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

        _memoryCache.Remove($"List-{id}");

        await UpdateAllListsInCache(list.UserId ?? throw new ArgumentNullException(list.UserId,
            "The list with the given id does not have a user id."));
    }

    private async Task UpdateAllListsInCache(string userId)
    {
        var cacheKey = $"User-{userId}-Lists";
        _memoryCache.Remove(cacheKey);
        _memoryCache.CreateEntry(cacheKey).Value = await _listRepository.GetAllAsync(userId);
    }
}