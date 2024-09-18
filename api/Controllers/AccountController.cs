using API.DTOs.AccountDTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Controller for handling account operations includes changing password, updating user information, and deleting an account.
/// </summary>
/// <code>
///     - GET    api/account/get-user-info/{id}
///     - PUT   api/account/change-password
///     - PUT   api/account/update-user-info
///     - DELETE api/account/delete-account/{id}
/// </code>
[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    /// <summary>
    ///     Constructor for the AccountController.
    /// </summary>
    public AccountController(IAccountService accountService) => _accountService = accountService;

    /// <summary>
    ///     Endpoint for getting the information of the authenticated user.
    /// </summary>
    /// <param name="id">
    ///     The id of the user to get the information.
    /// </param>
    /// <returns>
    ///     An IActionResult representing the result of the user information retrieval.
    ///     returns BadRequest if an exception is thrown,
    ///     otherwise returns Ok with the user information.
    /// </returns>
    [HttpGet("get-user-info/{id}")]
    public async Task<IActionResult> GetUserInfo([FromRoute] string id)
    {
        try
        {
            var userInfo = await _accountService.GetUserById(id);
            return Ok(userInfo);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Endpoint for changing the password of the authenticated user.
    /// </summary>
    /// <param name="changePasswordDto">
    ///     The credentials to use for changing the password (id, current password and new password).
    /// </param>
    /// <returns>
    ///     An IActionResult representing the result of the password change.
    ///     returns BadRequest if the ModelState is invalid or an exception is thrown,
    ///     otherwise returns Ok with a success message.
    /// </returns>
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto) // TODO: to be tested
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _accountService.ChangePassword(changePasswordDto);
            return result ? Ok("Password changed successfully.") : BadRequest("Failed to change password.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Endpoint for updating the information of the authenticated user.
    /// </summary>
    /// <param name="updateUserInfoDto">
    ///     The information to update for the authenticated user as FirstName, LastName, Email, UserName, and PhoneNumber.
    /// </param>
    /// <returns>
    ///     An IActionResult representing the result of the user information update.
    ///     returns BadRequest if the ModelState is invalid or an exception is thrown,
    ///     otherwise returns Ok with a success message.
    /// </returns>
    [HttpPut("update-user-info")]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoDto updateUserInfoDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _accountService.UpdateUserInfo(updateUserInfoDto);
            return result ? Ok("User information updated successfully.") : BadRequest("Failed to update user information.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    /// <summary>
    ///     Endpoint for deleting the account of the authenticated user.
    /// </summary>
    /// <param name="id">
    ///     The id of the user to delete the account.
    /// </param>
    /// <returns>
    ///     An IActionResult representing the result of the account deletion.
    ///     returns BadRequest if an exception is thrown,
    ///     returns BadRequest if the ModelState is invalid or an exception is thrown,
    ///     otherwise returns Ok with a success message.
    /// </returns>
    [HttpDelete("delete-account/{id}")]
    public async Task<IActionResult> DeleteAccount([FromRoute] string id) // TODO: to be tested
    {
        try
        {
            var result = await _accountService.DeleteAccount(id);
            return result ? Ok("Account deleted successfully.") : BadRequest("Failed to delete account.");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}