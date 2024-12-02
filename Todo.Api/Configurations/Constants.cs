namespace Todo.Api.Configurations;

public static class Constants
{
    public const string ConnectionStringName = "SqliteConnection";
    public const string JwtConfigurationsSectionKey = "JwtConfigurations";
    public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
    public const int TokenExpirationDays = 30;
    public const int RefreshTokenExpirationDays = 90;
}