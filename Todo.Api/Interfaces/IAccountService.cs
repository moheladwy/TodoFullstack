using System.Security.Claims;
using System.Threading.Tasks;
using Todo.Api.Models.DTOs.AccountDTOs;
using Todo.Api.Models.Entities;
using Task = System.Threading.Tasks.Task;

namespace Todo.Api.Interfaces;

/// <summary>
///     IAccountService interface is used to manage user operations like get user information,
///     changing password, updating user information etc.
/// </summary>
public interface IAccountService
{
    /// <summary>
    ///    GetUserById method is used to get user by id.
    /// </summary>
    /// <param name="userId">
    ///     The id of the user to get.
    /// </param>
    /// <returns>
    ///     Returns the user with the provided id.
    /// </returns>
    Task<User> GetUserById(string userId);

    /// <summary>
    ///     GetUserByClaims method is used to get user by claims.
    /// </summary>
    /// <param name="claims">
    ///     ClaimsPrincipal instance that contains user's claims.
    /// </param>
    /// <returns>
    ///     Returns the user with the provided claims.
    /// </returns>
    Task<User> GetUserByClaims(ClaimsPrincipal claims);

    /// <summary>
    ///     ChangePassword method is used to change user's password.
    /// </summary>
    /// <param name="changePasswordDto">
    ///     ChangePasswordDto instance that contains user's id, current password and new password.
    /// </param>
    /// <returns>
    ///     Returns true if password changed successfully.
    /// </returns>
    Task ChangePassword(ChangePasswordDto changePasswordDto);

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
    Task UpdateUserInfo(UpdateUserInfoDto updateUserInfoDto);

    /// <summary>
    ///     DeleteAccount method is used to delete the user account by id.
    /// </summary>
    /// <param name="id">
    ///     The id of the user to delete the account.
    /// </param>
    /// <returns>
    ///     Returns true if the account deleted successfully.
    /// </returns>
    Task DeleteAccount(string id);
}