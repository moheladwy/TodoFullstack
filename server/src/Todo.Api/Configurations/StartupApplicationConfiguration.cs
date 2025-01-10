using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.DatabaseContexts;

namespace Todo.Api.Configurations;

public static class StartupApplicationConfiguration
{
    /// <summary>
    ///     Automatically migrates the database at startup.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to use for the migration.
    /// </param>
    public static async void UseAutoMigrationAtStartup(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            var dbContext = services.GetRequiredService<TodoIdentityContext>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            logger.LogError(e, "This error occurred while migrating the database {Error}", e.Message);
        }
    }

    /// <summary>
    ///     Configures the application for development mode.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to configure.
    /// </param>
    public static void DevelopmentMode(this WebApplication app)
    {
        app.UseDeveloperExceptionPage();
        app.UseCors(Constants.ClientCrossOriginPolicyDevName);
    }

    /// <summary>
    ///     Configures the application for production mode.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to configure.
    /// </param>
    public static void ProductionMode(this WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.UseCors(Constants.ClientCrossOriginPolicyProductionName);
    }
}