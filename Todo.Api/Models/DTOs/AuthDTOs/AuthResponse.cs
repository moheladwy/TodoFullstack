namespace Todo.Api.Models.DTOs.AuthDTOs;

public class AuthResponse
{
    public string Id { get; set; } = string.Empty;
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpirationDate { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpirationDate { get; set; }
}