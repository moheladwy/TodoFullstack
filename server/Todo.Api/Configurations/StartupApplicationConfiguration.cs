using Microsoft.EntityFrameworkCore;
using Todo.Infrastructure.DatabaseContexts;

namespace Todo.Api.Configurations;

public static class StartupApplicationConfiguration
{
    /// <summary>
    ///    Initializes the application with the necessary services and middleware.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to initialize.
    /// </param>
    public static void InitializeConfigurations(this WebApplication app)
    {
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
    }

    /// <summary>
    ///     Automatically migrates the database at startup.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to use for the migration.
    /// </param>
    private static async void UseAutoMigrationAtStartup(this WebApplication app)
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
    private static void DevelopmentMode(this WebApplication app)
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
    private static void ProductionMode(this WebApplication app)
    {
        app.UseExceptionHandler("/error");
        app.UseCors(Constants.ClientCrossOriginPolicyProductionName);
    }
}