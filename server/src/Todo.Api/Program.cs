using FluentValidation;
using Todo.Api.Configurations;
using Todo.Api.Validators.Account;
using Todo.Infrastructure;

namespace Todo.Api;

/// <summary>
///     The main class that will be called when the application starts.
/// </summary>
public class Program
{
    /// <summary>
    ///     The main method that will be called when the application starts.
    /// </summary>
    /// <param name="args">
    ///     The arguments passed to the application.
    /// </param>
    public static void Main(string[] args)
    {
        // Create the builder.
        var builder = WebApplication.CreateBuilder(args);

        // Initialize all the services.
        builder.Services.AddValidatorsFromAssemblyContaining<UserDtoValidator>();

        builder.Services.AddEndpointsApiExplorer();
        builder.AddSwaggerService();
        builder.AddLoggingService();

        builder.AddSqlServerDb();
        builder.AddRedisConnectionString();
        builder.InitializeJwtConfigurations();
        builder.AddAuthenticationService();
        builder.Services.AddAuthorizationBuilder();
        builder.AddIdentityService();
        builder.AddCorsService();
        builder.RegisterServices();
        builder.RegisterRepositories();
        builder.RegisterCachingRepositories();

        builder.Services.AddHttpClient();
        builder.Services.AddControllers();

        // Build the app.
        var app = builder.Build();

        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseAutoMigrationAtStartup();

        if (app.Environment.IsDevelopment())
            app.DevelopmentMode();
        else
            app.ProductionMode();

        app.UseHttpsRedirection();
        app.UseHttpLogging();
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseStatusCodePages();
        app.MapControllers();

        // Run the app.
        app.Run();
    }
}