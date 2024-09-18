namespace API.DTOs.AuthDTOs;

public class AuthResponse
{
    public string? Id { get; set; } = string.Empty;
    public string? Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public int ExpiresInDays { get; set; }
}