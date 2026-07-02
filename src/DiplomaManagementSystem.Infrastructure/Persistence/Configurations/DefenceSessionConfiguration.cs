using DiplomaManagementSystem.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiplomaManagementSystem.Infrastructure.Persistence.Configurations;

internal sealed class DefenceSessionConfiguration : IEntityTypeConfiguration<DefenceSession>
{
    public void Configure(EntityTypeBuilder<DefenceSession> builder)
    {
        builder.ToTable("defence_sessions");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Year)
            .IsRequired();

        builder.Property(e => e.Type)
            .HasConversion<short>();

        builder.Property(e => e.Status)
            .HasConversion<short>();
    }
}
