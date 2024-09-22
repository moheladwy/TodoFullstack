using API.Services;

namespace API;

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
        builder.Initialize();

        // Build the app.
        var app = builder.Build();

        // Initialize all the middleware and services for the app.
        app.Initialize();

        // Run the app.
        app.Run();
    }
}