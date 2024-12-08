using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Entities;
using Task = Todo.Core.Entities.Task;

namespace Todo.Infrastructure.DatabaseContexts;

/// <summary>
///     The database context for the application that extends the IdentityDbContext.
/// </summary>
/// <param name="options">
///     The options to be passed to the base class.
/// </param>
public class TodoIdentityContext(DbContextOptions<TodoIdentityContext> options) : IdentityDbContext<User>(options)
{
    /// <summary>
    ///     The DbSet for the Task model in the database.
    /// </summary>
    public DbSet<Task> Tasks { get; init; }

    /// <summary>
    ///     The DbSet for the TaskList model in the database.
    /// </summary>
    public DbSet<TaskList> Lists { get; init; }

    /// <summary>
    ///     The OnModelCreating method that is called when the model is being created.
    /// </summary>
    /// <param name="modelBuilder">
    ///     The model builder that is used to build the model.
    /// </param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskList>()
            .HasMany(list => list.Tasks)
            .WithOne(task => task.TaskList)
            .HasForeignKey(task => task.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}