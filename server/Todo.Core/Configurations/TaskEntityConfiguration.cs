using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Enums;
using Task = Todo.Core.Entities.Task;

namespace Todo.Core.Configurations;

/// <summary>
///     The entity configuration for the Task entity.
/// </summary>
public class TaskEntityConfiguration : IEntityTypeConfiguration<Task>
{
    /// <summary>
    ///     Configures the entity of type Task.
    /// </summary>
    /// <param name="builder"> The builder to be used to configure the entity. </param>
    public void Configure(EntityTypeBuilder<Task> builder)
    {
        builder.HasKey(task => task.Id);
        builder.Property(task => task.Id).ValueGeneratedOnAdd();

        builder.Property(task => task.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(task => task.Description)
            .HasMaxLength(500);

        builder.Property(task => task.IsCompleted)
            .IsRequired();

        builder.Property(task => task.Priority)
            .HasColumnType("int")
            .HasDefaultValue(TaskPriority.Low)
            .IsRequired();

        builder.HasOne(task => task.TaskList)
            .WithMany(list => list.Tasks)
            .HasForeignKey(list => list.ListId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}