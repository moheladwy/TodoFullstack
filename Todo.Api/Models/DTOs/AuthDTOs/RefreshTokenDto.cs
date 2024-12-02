namespace Todo.Api.Models.DTOs.AuthDTOs;

public class RefreshTokenDto
{
    public string RefreshToken { get; set; }
    public DateTime ExpirationDate { get; set; }
}