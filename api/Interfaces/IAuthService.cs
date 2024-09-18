using API.DTOs.AuthDTOs;
using API.Models;

namespace API.Interfaces;

public interface IAuthService
{
    Task<bool> Register(RegisterUserDto registerUserDto);
    Task<User> Login(LoginUserDto loginUserDto);
    Task<bool> Logout();
}