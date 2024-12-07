namespace Todo.Infrastructure.Configurations;

/// <summary>
///     This class contains the roles that are used in the application.
///     The roles are used to restrict access to certain parts of the application.
///     The roles are used in the Authorize attribute in the controllers.
/// </summary>
public static class Roles
{
    /// <summary>
    ///     The User role is used to restrict access to certain parts of the application to only users.
    /// </summary>
    public const string User = "User";

    /// <summary>
    ///     The Admin role is used to restrict access to certain parts of the application to only administrators.
    /// </summary>
    public const string Admin = "Admin";
}