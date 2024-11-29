using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Configurations;
using API.Interfaces;
using api.Models.Entities;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace API.Services;

/// <summary>
///     Service for generating JWT tokens.
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    ///     The secret key used for generating JWT tokens.
    ///     This key is used to sign the JWT token.
    ///     The key is stored in the configuration.
    /// </summary>
    private readonly SymmetricSecurityKey _key;

    /// <summary>
    ///     The configuration to use for the TokenService.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    ///     The logger to use for the TokenService.
    /// </summary>
    private readonly ILogger<TokenService> _logger;

    /// <summary>
    ///     Constructor for the TokenService.
    /// </summary>
    /// <param name="configuration">
    ///     The configuration to use for the TokenService.
    /// </param>
    /// <param name="logger">
    ///     The logger to use for the TokenService.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the JWT Secret Key is not found in the configuration.
    /// </exception>
    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ??
                                                               throw new InvalidOperationException(
                                                                   "JWT Secret Key not found.")));
    }

    /// <summary>
    ///     Generates a JWT token for the specified user.
    /// </summary>
    /// <param name="user">
    ///     The user to generate the token for.
    /// </param>
    /// <returns>
    ///     A JWT token for the specified user.
    /// </returns>
    /// <exception cref="NullReferenceException">
    ///     Thrown when the user email or username is null.
    /// </exception>
    public string GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? throw new NullReferenceException("User email is null")),
            new(JwtRegisteredClaimNames.GivenName,
                user.UserName ?? throw new NullReferenceException("User username is null")),
            new(ClaimTypes.Role, Roles.User)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(Constants.TokenExpirationDays),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        _logger.LogInformation("Token generated for user: {user.Email}", user.Email);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    ///     Gets the secret key used for generating JWT tokens.
    /// </summary>
    /// <returns>
    ///     The secret key used for generating JWT tokens.
    /// </returns>
    public string GetSecretKey() => _key.ToString();

    /// <summary>
    ///     Gets the expiration days for the JWT token.
    /// </summary>
    /// <returns>
    ///     The expiration days for the JWT token.
    /// </returns>
    public int GetTokenExpirationDays() => Constants.TokenExpirationDays;
}