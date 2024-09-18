using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.DatabaseContexts;

public class TodoIdentityContext(DbContextOptions<TodoIdentityContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Models.Task> Tasks { get; init; }

    public DbSet<TaskList> Lists { get; init; }
}