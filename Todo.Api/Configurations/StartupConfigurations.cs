using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Todo.Infrastructure;
using Todo.Infrastructure.DatabaseContexts;

namespace Todo.Api.Configurations;

public static class StartupConfigurations
{
    /// <summary>
    ///     Initializes the application builder with the required services.
    /// </summary>
    /// <param name="builder">
    ///    The WebApplicationBuilder to initialize.
    /// </param>
    public static void InitializeBuilderConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.AddSwaggerService();
        builder.AddLoggingService();

        builder.AddDatabaseConnectionString();
        builder.InitializeJwtConfigurations();
        builder.AddAuthenticationService();
        builder.Services.AddAuthorizationBuilder();
        builder.AddIdentityService();
        builder.RegisterRepositories();
        builder.RegisterServices();

        builder.Services.AddHttpClient();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler =
                System.Text.Json.Serialization.ReferenceHandler.Preserve;
        });
    }

    /// <summary>
    ///     Adds the Swagger service to the application builder for API documentation with JWT authentication.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the Swagger service to.
    /// </param>
    private static void AddSwaggerService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter your JWT token in this field: {your token}",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            };

            c.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            };

            c.AddSecurityRequirement(securityRequirement);
        });
    }

    /// <summary>
    ///     Adds the logging service to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the logging service to.
    /// </param>
    private static void AddLoggingService(this WebApplicationBuilder builder)
    {
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddConsole()
                .AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug)
                .AddFilter("Microsoft.AspNetCore.Authorization", LogLevel.Debug);
        });
        builder.Services.AddHttpLogging(configureOptions: options =>
        {
            options.LoggingFields = HttpLoggingFields.All;
            options.RequestBodyLogLimit = 4096;
            options.ResponseBodyLogLimit = 4096;
            options.RequestHeaders.Add("Authorization");
            options.ResponseHeaders.Add("Authorization");
        });
    }
    
    /// <summary>
    ///    Initializes the application with the necessary services and middleware.
    /// </summary>
    /// <param name="app">
    ///     The WebApplication instance to initialize.
    /// </param>
    public static void InitializeApplicationConfigurations(this WebApplication app)
    {
        app.UseAutoMigrationAtStartup();

        if (app.Environment.IsDevelopment())
            app.DevelopmentMode();
        else
            app.ProductionMode();

        app.UseHttpsRedirection();
        // app.UseHttpLogging();
        app.UseMiddleware<GlobalExceptionMiddleware>();
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
        try
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<TodoIdentityContext>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            var logger = app.Services.GetRequiredService<LoggerFactory>().CreateLogger("Database Migration");
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