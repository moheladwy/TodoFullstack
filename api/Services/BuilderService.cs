using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using API.DatabaseContexts;
using api.DTOs.ListDTOs;
using API.DTOs.TasksDTOs;
using API.Interfaces;
using API.Models;
using API.Repository;
using Models_Task = API.Models.Task;

namespace API.Services;

/// <summary>
///    Service for configuring the application builder.
/// </summary>
public static class BuilderService
{
    /// <summary>
    ///     Initializes the application builder with the required services.
    /// </summary>
    /// <param name="builder">
    ///    The WebApplicationBuilder to initialize.
    /// </param>
    public static void Initialize(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();

        builder.AddDatabaseConnectionString();
        builder.AddSwaggerService();

        builder.AddAuthenticationService();
        builder.AddAuthorizationService();
        builder.AddLoggingService();

        builder.AddIdentityService();
        builder.AddRepositoryService();

        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthService, AuthenticationService>();
        builder.Services.AddScoped<IAccountService, AccountService>();
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
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API | V1.2", Version = "v1" });
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
    ///     Adds the authentication service to the application builder with JWT authentication.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the authentication service to.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the JWT Signing Key is not found in the configuration.
    /// </exception>
    private static void AddAuthenticationService(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"] ??
                                                       throw new InvalidOperationException("JWT Secret Key not found."))
                ),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    /// <summary>
    ///     Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the database connection string to.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection string is not found in the configuration.
    /// </exception>
    private static void AddDatabaseConnectionString(this WebApplicationBuilder builder)
    {
        const string connectionStringName = "SqliteConnection";
        var connectionString = builder.Configuration.GetConnectionString(connectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string '{connectionStringName}' not found.");
        builder.Services.AddDbContext<TodoIdentityContext>(options => options.UseSqlite(connectionString));
    }

    /// <summary>
    ///     Adds the Identity service to the application builder.
    ///     Configures the Identity options for the application.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the Identity service to.
    /// </param>
    private static void AddIdentityService(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;

            options.Password.RequiredLength = 12;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredUniqueChars = 0;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<TodoIdentityContext>();
        // .AddRoles<IdentityRole>();
    }

    /// <summary>
    ///     Adds the authorization service to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the authorization service to.
    /// </param>
    private static void AddAuthorizationService(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder();
        // .AddPolicy(Roles.Admin, policy => policy.RequireRole(Roles.Admin))
        // .AddPolicy(Roles.User, policy => policy.RequireRole(Roles.User));
    }

    /// <summary>
    ///     Adds the repository service to the application builder.
    ///     Adds the TasksRepository and ListRepository to the services.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the repository service to.
    /// </param>
    private static void AddRepositoryService(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRepository<TaskList, AddListDto, UpdateListDto>, ListRepository>();
        builder.Services.AddScoped<IRepository<Models_Task, AddTaskDto, UpdateTaskDto>, TasksRepository>();
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
    }
}