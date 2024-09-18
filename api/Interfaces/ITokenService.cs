using API.Models;

namespace API.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
    string GetSecretKey();
    int GetTokenExpirationDays();
}