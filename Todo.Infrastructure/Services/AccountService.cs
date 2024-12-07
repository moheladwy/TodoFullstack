using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Todo.Core.DTOs.AccountDTOs;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Services;

/// <summary>
///     AccountService class is used to manage user operations like changing password, updating user information etc.
/// </summary>
public class AccountService : IAccountService
{
    /// <summary>
    ///     UserManager instance to manage user operations.
    /// </summary>
    private readonly UserManager<User> _userManager;

    /// <summary>
    ///     ILogger instance to log information, warnings, and errors.
    /// </summary>
    private readonly ILogger<AccountService> _logger;

    /// <summary>
    ///     Constructor of AccountService class.
    /// </summary>
    /// <param name="userManager">
    ///     UserManager instance to manage user operations, it's injected by dependency injection container.
    /// </param>
    /// <param name="signInManager">
    ///     SignInManager instance to manage sign in operations, it's injected by dependency injection container.
    /// </param>
    /// <param name="logger">
    ///    ILogger instance to log information, warnings, and errors, it's injected by dependency injection container.
    ///    It's used to log information, warnings, and errors.
    /// </param>
    public AccountService(UserManager<User> userManager, ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    ///     GetUserById method is used to get user by id.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user to get.
    /// </param>
    /// <returns>
    ///     Returns user if user found by given id.
    /// </returns>
    /// <exception cref="UserNotFoundException">
    ///     Throws when user not found by given id.
    /// </exception>
    public async Task<User> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId) ??
                   throw new UserNotFoundException($"User with this id: {userId} not found");
        return user;
    }

    /// <summary>
    ///     GetUserByClaims method is used to get user by claims.
    /// </summary>
    /// <param name="claims">
    ///     ClaimsPrincipal instance that contains user's claims.
    /// </param>
    /// <returns>
    ///     Returns user if user found by given claims.
    /// </returns>
    /// <exception cref="UserNotFoundException">
    ///     Throws when user not found by given claims.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    ///     This sample shows how to call the GetUserByClaims method.
    /// </exception>
    public async Task<User> GetUserByClaims(ClaimsPrincipal claims)
    {
        var userEmail = claims.FindFirst(ClaimTypes.Email)?.Value ?? 
                        throw new UnauthorizedAccessException("User not authenticated.");

        var user = await _userManager.FindByEmailAsync(userEmail) ??
            throw new UserNotFoundException($"User with this email: {userEmail} not found");
        
        return user;
    }

    /// <summary>
    ///     ChangePassword method is used to change user's password.
    /// </summary>
    /// <param name="changePasswordDto">
    ///     ChangePasswordDto instance that contains user's id, current password and new password.
    /// </param>
    /// <returns>
    ///     Returns true if password changed successfully.
    /// </returns>
    /// <exception cref="UserNotFoundException">
    ///     Throws when user not found by given id.
    /// </exception>
    /// <exception cref="PasswordDidNotChangeException">
    ///     Throws when password did not change successfully.
    /// </exception>
    public async Task ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var user = await GetUserById(changePasswordDto.Id);

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword,
                changePasswordDto.NewPassword);
        if (!result.Succeeded)
            throw new PasswordDidNotChangeException($"Failed to change password because of: {string.Join(", ",
                result.Errors.Select(e => e.Description))}");
    }

    /// <summary>
    ///     UpdateUserInfo method is used to update user's information like first name, last name, email, username, and phone number.
    ///     It updates only the provided properties and keeps the others as they are if they are not provided (null or empty).
    ///     It does not update the password, to update the password use ChangePassword method.
    ///     And it does not update the user's id.
    /// </summary>
    /// <param name="updateUserInfoDto">
    ///     UpdateUserInfoDto instance that contains user's id and new information.
    ///     New information can be new first name, new last name, new email, new username, and new phone number.
    /// </param>
    /// <returns>
    ///     Returns true if user information updated successfully.
    /// </returns>
    /// <exception cref="UserNotFoundException">
    ///     Throws when user not found by given id.
    /// </exception>
    /// <exception cref="UserInformationDidNotUpdateException">
    ///     Throws when user information did not update successfully.
    /// </exception>
    public async Task UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto)
    {
        var user = await GetUserById(updateUserInfoDto.Id);
        
        UpdateUserInformation(user, updateUserInfoDto);
        
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new UserInformationDidNotUpdateException($"Failed to update user information because of: {
                string.Join(", ", result.Errors.Select(e => e.Description))}");
    }
    
    private static void UpdateUserInformation(User user, UpdateUserInfoDto updateUserInfoDto)
    {
        // Update the user's first name if it's provided
        if (!string.IsNullOrEmpty(updateUserInfoDto.NewFirstName))
            user.FirstName = updateUserInfoDto.NewFirstName;

        // Update the user's last name if it's provided
        if (!string.IsNullOrEmpty(updateUserInfoDto.NewLastName))
            user.LastName = updateUserInfoDto.NewLastName;

        // Update the user's email if it's provided
        if (!string.IsNullOrEmpty(updateUserInfoDto.NewEmail))
            user.Email = updateUserInfoDto.NewEmail;

        // Update the user's username if it's provided
        if (!string.IsNullOrEmpty(updateUserInfoDto.NewUserName))
            user.UserName = updateUserInfoDto.NewUserName;

        // Update the user's phone number if it's provided
        if (!string.IsNullOrEmpty(updateUserInfoDto.NewPhoneNumber))
            user.PhoneNumber = updateUserInfoDto.NewPhoneNumber;
    }

    /// <summary>
    ///     DeleteAccount method is used to delete user account by id.
    /// </summary>
    /// <param name="id">
    ///     The id of the user to delete account.
    /// </param>
    /// <returns>
    ///     Returns true if user account deleted successfully.
    ///     Returns false if user account not deleted successfully.
    /// </returns>
    /// <exception cref="UserNotFoundException">
    ///     Throws when user not found by given id.
    /// </exception>
    public async Task DeleteAccount(string id)
    {
        var user = await GetUserById(id);
        await _userManager.DeleteAsync(user);
    }
}