using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.Core.DTOs.ListDTOs;
using Todo.Core.DTOs.TasksDtos;
using Todo.Core.Entities;
using Todo.Core.Interfaces;
using Todo.Infrastructure.Configurations;
using Todo.Infrastructure.DatabaseContexts;
using Todo.Infrastructure.Repositories;
using Todo.Infrastructure.Services;
using Task = Todo.Core.Entities.Task;

namespace Todo.Infrastructure;

public static class Extensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthenticationService>();
        services.AddScoped<IAccountService, AccountService>();
    }

    public static void RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<TaskList, AddListDto, UpdateListDto>, ListRepository>();
        services.AddScoped<IRepository<Task, AddTaskDto, UpdateTaskDto>, TasksRepository>();
    }

    /// <summary>
    ///     Adds the database connection string to the application builder.
    /// </summary>
    /// <param name="services">
    ///     The service collection.
    /// </param>
    /// <param name="configuration">
    ///    The configuration object.
    /// </param>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the connection string is not found in the configuration.
    /// </exception>
    public static void AddDatabaseConnectionString(this IServiceCollection services, IConfigurationManager configuration)
    {
        var connectionString = configuration.GetConnectionString(Constants.ConnectionStringName)
                               ?? throw new InvalidOperationException(
                                   $"Connection string '{Constants.ConnectionStringName}' not found.");
        services.AddDbContext<TodoIdentityContext>(options => options.UseSqlite(connectionString));
    }
}