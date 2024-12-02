namespace Todo.Api.Configurations;

public class JwtConfigurations
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience {  get; set; }
    public int AccessTokenExpirationDays { get; set; }
    public int RefreshTokenExpirationDays {  get; set; }
}