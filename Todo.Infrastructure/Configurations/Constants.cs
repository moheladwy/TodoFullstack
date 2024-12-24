namespace Todo.Infrastructure.Configurations;

public static class Constants
{
    public const string ConnectionStringName = "SqliteConnection";
    public const string RedisConnectionStringName = "RedisConnection";
    public const string JwtConfigurationsSectionKey = "JwtConfigurations";
    public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
    public const int TimeSpanByMinutesForCaching = 5;
}