using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Core.Configurations;

/// <summary>
///     The entity configuration for the User entity.
/// </summary>
public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    /// <summary>
    ///     Configures the entity of type Task.
    /// </summary>
    /// <param name="builder"> The builder to be used to configure the entity. </param>
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(user => user.FirstName)
            .IsRequired()
            .HasMaxLength(25);

        builder.Property(user => user.LastName)
            .IsRequired()
            .HasMaxLength(25);

        builder.HasMany(user => user.Lists)
            .WithOne(list => list.User)
            .HasForeignKey(list => list.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}