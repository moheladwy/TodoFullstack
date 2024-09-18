using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace API.DatabaseContexts;

public class TodoIdentityContext : IdentityDbContext<User>
{
    public DbSet<Models.Task> Tasks { get; init; }

    public DbSet<TaskList> Lists { get; init; }

    public TodoIdentityContext(DbContextOptions<TodoIdentityContext> options) : base(options) { }
}