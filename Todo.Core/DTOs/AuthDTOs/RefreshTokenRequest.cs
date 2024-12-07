namespace Todo.Core.DTOs.AuthDTOs;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}