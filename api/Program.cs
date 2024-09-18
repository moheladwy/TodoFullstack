using API.Services;

namespace API;

// This is the entry point for the application.
public static class Program
{
    // This is the main method that will be called when the application starts.
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