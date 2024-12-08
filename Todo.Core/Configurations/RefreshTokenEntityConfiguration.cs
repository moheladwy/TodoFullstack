using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Core.Entities;

namespace Todo.Core.Configurations;

/// <summary>
///     Configuration for the RefreshToken entity.
///     - Defines the database schema for the RefreshToken entity.
/// </summary>
public class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    /// <summary>
    ///     Configures the RefreshToken entity's properties, relationships, and indexes.
    /// </summary>
    /// <param name="builder">The builder to use to configure the RefreshToken entity. </param>
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Token).IsRequired();
        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.Property(rt => rt.ExpirationDate).IsRequired();
        builder.Property(rt => rt.UserId).IsRequired(false);

        // Define the RefreshToken entity's relationships.
        builder.HasOne(rt => rt.User)
            .WithOne(u => u.RefreshToken)
            .HasForeignKey<RefreshToken>(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}