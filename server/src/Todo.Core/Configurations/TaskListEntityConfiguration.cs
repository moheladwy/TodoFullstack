using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Core.Configurations;

/// <summary>
///     The entity configuration for the TaskList entity.
/// </summary>
public class TaskListEntityConfiguration : IEntityTypeConfiguration<TaskList>
{
    /// <summary>
    ///     Configures the entity of type Task.
    /// </summary>
    /// <param name="builder"> The builder to be used to configure the entity. </param>
    public void Configure(EntityTypeBuilder<TaskList> builder)
    {
        builder.HasKey(list => list.Id);
        builder.Property(list => list.Id).ValueGeneratedOnAdd();

        builder.Property(list => list.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(list => list.Description)
            .HasMaxLength(500);

        builder.HasMany(list => list.Tasks)
            .WithOne(task => task.TaskList)
            .HasForeignKey(task => task.ListId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}