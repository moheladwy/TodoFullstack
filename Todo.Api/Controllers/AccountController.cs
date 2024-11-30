using Todo.Api.Models.DTOs.AccountDTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Api.Exceptions;
using Todo.Api.Interfaces;

namespace Todo.Api.Controllers;

/// <summary>
///     Controller for handling account operations includes changing password, updating user information, and deleting an account.
/// </summary>
/// <code>
///     - GET    api/account/get-user/{id}
///     - PUT    api/account/change-password
///     - PUT    api/account/update-user-info
///     - DELETE api/account/delete-account/{id}
/// </code>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    /// <summary>
    ///     Constructor for the AccountController.
    /// </summary>
    /// <param name="accountService">
    ///     The service to use for account operations.
    /// </param>
    /// <param name="logger">
    ///     The logger to use for logging.
    /// </param>
    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }

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
    [HttpGet("get-user/{id}")]
    public async Task<IActionResult> GetUserById([FromRoute] string id)
    {
        var userInfo = await _accountService.GetUserById(id);
        _logger.LogInformation("User information retrieved successfully for user with id: {id}", userInfo.Id);
        return Ok(userInfo);
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
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Failed to change password because: {ModelState.ValidationState}");

        await _accountService.ChangePassword(changePasswordDto);
        
        _logger.LogInformation("Password changed successfully for user with id: {id}", changePasswordDto.Id);
        return Ok("Password changed successfully.");
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
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Failed to update user information because: {ModelState.ValidationState}");

        await _accountService.UpdateUserInfo(updateUserInfoDto);
        
        _logger.LogInformation("User information updated successfully for user with id: {id}", updateUserInfoDto.Id);
        return Ok("User information updated successfully.");
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
    public async Task<IActionResult> DeleteAccount([FromRoute] string id)
    {
        await _accountService.DeleteAccount(id);
        
        _logger.LogInformation("Account deleted successfully for user with id: {id}", id);
        return Ok("Account deleted successfully.");
    }
}