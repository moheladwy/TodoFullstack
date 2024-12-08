using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using SystemNewFile1.txt.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;

namespace Todo.Infrastructure.Services;

/// <summary>
///     Service for generating JWT tokens.
/// </summary>
public class TokenService : ITokenService
{
    /// <summary>
    ///     The logger to use for the TokenService.
    /// </summary>
    private readonly ILogger<TokenService> _logger;

    /// <summary>
    ///     The JWT configurations to use for the TokenService.
    /// </summary>
    private readonly JwtConfigurations _jwtConfigurations;

    /// <summary>
    ///     Constructor for the TokenService.
    /// </summary>
    /// <param name="logger">
    ///     The logger to use for the TokenService.
    /// </param>
    /// <param name="jwtConfigurations">
    ///     The JWT configurations to use for the TokenService.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the JWT Secret Key is not found in the configuration.
    /// </exception>
    public TokenService(ILogger<TokenService> logger, IOptions<JwtConfigurations> jwtConfigurations)
    {
        _logger = logger;
        _jwtConfigurations = jwtConfigurations.Value;
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
    public AccessTokenDto GenerateToken(User user)
    {
        ArgumentNullException.ThrowIfNull(user, nameof(user));
        var claims = InitializeClaimsList(user);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(_jwtConfigurations.AccessTokenExpirationDays),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
            Issuer = _jwtConfigurations.Issuer,
            Audience = _jwtConfigurations.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        var accessToken = new AccessTokenDto()
        {
            Token = tokenHandler.WriteToken(token),
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtConfigurations.AccessTokenExpirationDays)
        };

        _logger.LogInformation("JWT token generated for user: {user.Email}", user.Email);
        return accessToken;
    }

    private static List<Claim> InitializeClaimsList(User user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Jti, user.Id),
            new Claim(JwtRegisteredClaimNames.Email,
                user.Email ?? throw new NullReferenceException("User email is null")),
            new Claim(JwtRegisteredClaimNames.GivenName,
                user.UserName ?? throw new NullReferenceException("User username is null")),
            new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
            new Claim(JwtRegisteredClaimNames.Exp,
                new DateTimeOffset(DateTime.Now.AddDays(Constants.TokenExpirationDays)).ToUnixTimeSeconds().ToString()),
            new Claim(ClaimTypes.Role, Roles.User)
        ];
    }

    public RefreshTokenDto GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var refreshToken = new RefreshTokenDto
        {
            RefreshToken = Convert.ToBase64String(randomNumber),
            ExpirationDate = DateTime.UtcNow.AddDays(_jwtConfigurations.RefreshTokenExpirationDays)
        };

        _logger.LogInformation("Refresh token generated.");
        return refreshToken;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var secretKey = Encoding.UTF8.GetBytes(_jwtConfigurations.SecretKey);

        var validation = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtConfigurations.Issuer,
            ValidAudience = _jwtConfigurations.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            ValidateLifetime = false,
        };

        return new JwtSecurityTokenHandler().ValidateToken(token, validation, out _);
    }
}