namespace Projekt.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;

public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
        builder.Property(d => d.Description).HasMaxLength(1000);
        builder.Property(d => d.Percentage).IsRequired();
        builder.Property(d => d.StartDate).IsRequired();
        builder.Property(d => d.EndDate).IsRequired();
        
        builder.HasOne(d => d.Software)
            .WithMany(s => s.Discounts)
            .HasForeignKey(d => d.SoftwareId);
    }
}
