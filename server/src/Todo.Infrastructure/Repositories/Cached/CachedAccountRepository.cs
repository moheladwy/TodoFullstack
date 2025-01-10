using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Todo.Core.DTOs.AccountDTOs;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Repositories.DB;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Repositories.Cached;

public class CachedAccountRepository : IAccountRepository
{
    private readonly AccountRepository _accountRepository;
    private readonly IRedisCacheService _cacheService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CachedAccountRepository"/> class.
    /// </summary>
    /// <param name="accountRepository"></param>
    /// <param name="cacheService"></param>
    public CachedAccountRepository(AccountRepository accountRepository, IRedisCacheService cacheService)
    {
        _accountRepository = accountRepository;
        _cacheService = cacheService;
    }

    public async Task<User> GetUserById(string userId)
    {
        var cacheKey = $"User-{userId}";

        var cachedUser = await _cacheService.GetData<User>(cacheKey);
        if (cachedUser is not null) return cachedUser;

        var user = await _accountRepository.GetUserById(userId);
        await _cacheService.SetData(cacheKey, user);
        return user;
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
        await _cacheService.UpdateData($"User-{changePasswordDto.Id}",
            await _accountRepository.GetUserById(changePasswordDto.Id));
    }

    public async Task<UserDto> UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
    {
        var user = await _accountRepository.UpdateUserInfo(updateUserInfoDto);
        await _cacheService.UpdateData($"User-{updateUserInfoDto.Id}",
            await _accountRepository.GetUserById(updateUserInfoDto.Id));
        return user;
    }

    public async Task DeleteAccount(string id)
    {
        await _accountRepository.DeleteAccount(id);
        await _cacheService.RemoveData($"User-{id}");
    }
}