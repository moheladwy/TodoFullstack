using Microsoft.AspNetCore.HttpLogging;
using Microsoft.OpenApi.Models;
using Todo.Infrastructure;

namespace Todo.Api.Configurations;

public static class StartupBuilderConfigurations
{
    /// <summary>
    ///     Initializes the application builder with the required services.
    /// </summary>
    /// <param name="builder">
    ///    The WebApplicationBuilder to initialize.
    /// </param>
    public static void InitializeConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.AddSwaggerService();
        builder.AddLoggingService();

        builder.AddDatabaseConnectionString();
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
    }

    /// <summary>
    ///    Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder"></param> <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void AddCorsService(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(Constants.ClientCrossOriginPolicyDevName, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            options.AddPolicy(Constants.ClientCrossOriginPolicyProductionName, builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
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
}