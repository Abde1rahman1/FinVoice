using FinVoice.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinVoice.Presistence.Configurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.MonthlyLimit)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");
    }
}
