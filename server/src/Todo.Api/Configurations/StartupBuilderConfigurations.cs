using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Todo.Core.Entities;
using Todo.Infrastructure.Configurations;
using Todo.Infrastructure.DatabaseContexts;

namespace Todo.Api.Configurations;

public static class StartupBuilderConfigurations
{
    /// <summary>
    ///     Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection string is not found in the configuration.
    /// </exception>
    public static void AddSqliteDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(Constants.SqliteConnectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string '{Constants.SqliteConnectionStringName}' not found.");
        builder.Services.AddDbContext<TodoIdentityContext>(options => options.UseSqlite(connectionString));
    }

    /// <summary>
    ///     Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///    Thrown when the connection string is not found in the configuration.
    /// </exception>
    public static void AddSqlServerDb(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(Constants.SqlServerConnectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string '{Constants.SqlServerConnectionStringName}' not found.");
        builder.Services.AddDbContext<TodoIdentityContext>(options => options.UseSqlServer(connectionString));
    }

    /// <summary>
    ///     Adds the Redis connection string to the application builder.
    /// </summary>
    /// <param name="builder">
    ///     The WebApplicationBuilder to initialize.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection string is not found in the configuration.
    /// </exception>
    public static void AddRedisConnectionString(this WebApplicationBuilder builder)
    {
        var redisConnectionString = builder.Configuration.GetConnectionString(Constants.RedisConnectionStringName)
                                   ?? throw new InvalidOperationException(
                                       $"Connection string '{Constants.RedisConnectionStringName}' not found.");
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "TodoFullStack_";
        });
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
    public static void AddSwaggerService(this WebApplicationBuilder builder)
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
    public static void AddLoggingService(this WebApplicationBuilder builder)
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