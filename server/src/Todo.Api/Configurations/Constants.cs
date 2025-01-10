namespace Todo.Api.Configurations;

public static class Constants
{
  public const string SqliteConnectionStringName = "SqliteConnection";
  public const string SqlServerConnectionStringName = "SqlServerConnection";
  public const string RedisConnectionStringName = "RedisConnection";
  public const string JwtConfigurationsSectionKey = "JwtConfigurations";
  public const string ClientCrossOriginPolicyProductionName = "AllowProductionReactApp";
  public const string ClientCrossOriginPolicyDevName = "AllowDevelopmentReactApp";
  public const string ClientCrossOriginPolicyDevURL = "http://localhost:5173";
  public const string ClientCrossOriginPolicyProductionURL = "https://todo.aladawy.duckdns.org";
  public const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
}