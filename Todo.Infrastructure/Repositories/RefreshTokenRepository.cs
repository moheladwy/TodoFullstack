using Microsoft.EntityFrameworkCore;
using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.DatabaseContexts;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    /// <summary>
    ///     The TodoIdentityContext instance to use for database operations.
    /// </summary>
    private readonly TodoIdentityContext _context;
    
    /// <summary>
    ///     Constructor for the RefreshTokenRepository class.
    /// </summary>
    /// <param name="context">
    ///     The TodoIdentityContext instance to use for database operations, it's registered in the DI container.
    /// </param>
    public RefreshTokenRepository(TodoIdentityContext context) => _context = context;

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
           throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null or empty.");
        
        return await _context.RefreshTokens
            .Where(x => x.Token == refreshToken)
            .FirstOrDefaultAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null or empty.");

        return await _context.RefreshTokens
            .Where(t => t.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId)
    {
        if (refreshTokenId == Guid.Empty)
            throw new ArgumentNullException(nameof(refreshTokenId), "Refresh token ID cannot be empty.");

        return await _context.RefreshTokens.FindAsync(refreshTokenId);
    }

    public async Task<RefreshToken> AddRefreshTokenAsync(RefreshTokenDto refreshToken)
    {
        var refreshTokenEntity = new RefreshToken
        {
            Id = new Guid(),
            Token = refreshToken.RefreshToken,
            ExpirationDate = refreshToken.RefreshTokenExpirationDate,
            UserId = refreshToken.UserId
        };
        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync();
        return refreshTokenEntity;
    }

    public async Task DeleteRefreshTokenAsync(Guid refreshTokenId)
    {
        if (refreshTokenId == Guid.Empty)
            throw new ArgumentNullException(nameof(refreshTokenId), "Refresh token ID cannot be empty.");

        var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);
        if (refreshToken == null)
            throw new ArgumentNullException(nameof(refreshTokenId), "Refresh token not found.");

        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
    }
}