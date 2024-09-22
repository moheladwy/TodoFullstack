using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.DatabaseContexts;
using API.Models;

namespace API.Services;

/// <summary>
///     Service for initializing the application with the necessary services and middleware.
/// </summary>
public static class ApplicationService
{
    /// <summary>
    ///    Initializes the application with the necessary services and middleware.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to initialize.
    /// </param>
    public static void Initialize(this WebApplication app)
    {
        app.UseAutoMigrationAtStartup();

        if (app.Environment.IsDevelopment())
            app.DevelopmentMode();
        else
            app.ProductionMode();

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthentication();
        app.UseAuthorization();
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
        var dbContext = services.GetRequiredService<TodoIdentityContext>();
        await dbContext.Database.MigrateAsync();
    }

    /// <summary>
    ///     Adds the initial roles to the application.
    ///     The roles are created if they don't exist.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to use for adding the roles.
    /// </param>
    private static async void AddInitialRoles(this WebApplication app)
    {
        // Get RoleManager service
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Define the roles that you need in your system
        string[] roleNames = [Roles.User, Roles.Admin];

        // Loop through the role names and create each if it doesn't exist
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) await roleManager.CreateAsync(new IdentityRole(roleName));
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
    }
}