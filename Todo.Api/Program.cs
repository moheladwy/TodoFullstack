using Microsoft.AspNetCore.Builder;
using Todo.Api.Configurations;

namespace Todo.Api;

/// <summary>
///     The main class that will be called when the application starts.
/// </summary>
public static class Program
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
        builder.InitializeBuilder();

        // Build the app.
        var app = builder.Build();

        // Initialize all the middleware and services for the app.
        app.InitializeApplication();

        // Run the app.
        app.Run();
    }
}