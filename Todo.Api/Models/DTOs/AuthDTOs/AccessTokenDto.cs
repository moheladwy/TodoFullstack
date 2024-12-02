namespace Todo.Api.Models.DTOs.AuthDTOs;

public class AccessTokenDto
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}