using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Todo.Core.DTOs.ListDTOs;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;
using Todo.Infrastructure.DatabaseContexts;
using Todo.Infrastructure.Repositories.Cached;
using Todo.Infrastructure.Repositories.DB;
using Todo.Infrastructure.Services;
using Task = Todo.Core.Entities.Task;

namespace Todo.Infrastructure;

public static class Extensions
{
    public static void RegisterServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthService, AuthenticationService>();
    }

    public static void RegisterRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ListRepository>();
        builder.Services.AddScoped<TasksRepository>();
        builder.Services.AddScoped<AccountRepository>();
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    public static void RegisterCachingRepositories(this WebApplicationBuilder builder)
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString(Constants.RedisConnectionStringName);
            options.InstanceName = "TodoFullStack_";
        });
        builder.Services.AddScoped<IRedisCacheService, RedisCachingService>();
        builder.Services.AddScoped<IAccountRepository, CachedAccountRepository>();
        builder.Services.AddScoped<IRepository<TaskList, AddListDto, UpdateListDto>, CachedListRepository>();
        builder.Services.AddScoped<IRepository<Task, AddTaskDto, UpdateTaskDto>, CachedTasksRepository>();
    }

    /// <summary>
    ///     Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection string is not found in the configuration.
    /// </exception>
    public static void AddDatabaseConnectionString(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(Constants.ConnectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string '{Constants.ConnectionStringName}' not found.");
        builder.Services.AddDbContext<TodoIdentityContext>(options => options.UseSqlite(connectionString));
    }

    /// <summary>
    ///     Initializes the JWT configurations for the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    public static void InitializeJwtConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<JwtConfigurations>(
            builder.Configuration.GetSection(Constants.JwtConfigurationsSectionKey));
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
    public static void AddAuthenticationService(this WebApplicationBuilder builder)
    {
        var jwtConfigurations = new JwtConfigurations();
        builder.Configuration.GetSection(Constants.JwtConfigurationsSectionKey).Bind(jwtConfigurations);

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
                ValidIssuer = jwtConfigurations.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfigurations.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfigurations.SecretKey)
                ),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    /// <summary>
    ///     Adds the Identity service to the application builder.
    ///     Configures the Identity options for the application.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to add the Identity service to.
    /// </param>
    public static void AddIdentityService(this WebApplicationBuilder builder)
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

            options.User.AllowedUserNameCharacters = Constants.AllowedUserNameCharacters;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<TodoIdentityContext>();
        // .AddRoles<IdentityRole>();
    }
}