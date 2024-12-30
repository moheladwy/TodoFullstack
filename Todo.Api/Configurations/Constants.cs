namespace Todo.Api.Configurations;

public static class Constants
{
  public const string ConnectionStringName = "SqliteConnection";
  public const string RedisConnectionStringName = "RedisConnection";
  public const string RedisInstanceName = "TodoFullStack_";
  public const string JwtConfigurationsSectionKey = "JwtConfigurations";
  public const string AuthGoogleClientId = "Authentication:Google:ClientId";
  public const string AuthGoogleClientSecret = "Authentication:Google:ClientSecret";
  public const string AuthGoogleCallbackPath = "Authentication:Google:CallbackPath";
  public const string CookieLoginPath = "/api/Auth/google/signin";
  public const string CookieName = "Todo.Auth.Google";
  public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
}
