using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Todo.Core.DTOs.AccountDTOs;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Repositories;

public class CachedAccountRepository : IAccountRepository
{
    private readonly AccountRepository _accountRepository;
    private readonly IMemoryCache _memoryCache;

    public CachedAccountRepository(AccountRepository accountRepository, IMemoryCache memoryCache)
    {
        _accountRepository = accountRepository;
        _memoryCache = memoryCache;
    }

    public async Task<User> GetUserById(string userId)
    {
        var cacheKey = $"User-{userId}";

        var cachedUser = await _memoryCache.GetOrCreateAsync(
            cacheKey,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(Constants.TimeSpanByMinutesForCaching));
                return _accountRepository.GetUserById(userId);
            });

        return cachedUser ?? throw new UserNotFoundException($"User with id: {userId} not found");
    }

    public async Task<User> GetUserByClaims(ClaimsPrincipal claims)
    {
        var userId = claims.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ??
                     throw new UnauthorizedAccessException("User not authenticated.");
        return await GetUserById(userId);
    }

    public async Task ChangePassword(ChangePasswordDto changePasswordDto)
    {
        await _accountRepository.ChangePassword(changePasswordDto);

        var cacheKey = $"User-{changePasswordDto.Id}";
        _memoryCache.Remove(cacheKey);
        _memoryCache.CreateEntry(cacheKey).Value = await _accountRepository.GetUserById(changePasswordDto.Id);
    }

    public async Task UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
    {
        await _accountRepository.UpdateUserInfo(updateUserInfoDto);

        var cacheKey = $"User-{updateUserInfoDto.Id}";
        _memoryCache.Remove(cacheKey);
        _memoryCache.CreateEntry(cacheKey).Value = await _accountRepository.GetUserById(updateUserInfoDto.Id);
    }

    public async Task DeleteAccount(string id)
    {
        await _accountRepository.DeleteAccount(id);
        var cacheKey = $"User-{id}";
        _memoryCache.Remove(cacheKey);
    }
}