using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace Todo.Core.Interfaces;

public interface IAuthService
{
    Task<User> Register(RegisterUserDto registerUserDto);
    Task<User> Login(LoginUserDto loginUserDto);
    Task<AuthResponse> RefreshToken(RefreshTokenRequest request);
    Task Logout(string username);
}