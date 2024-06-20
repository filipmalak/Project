namespace Projekt.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;

public class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.BasePrice).IsRequired();
        builder.Property(c => c.DiscountedPrice).IsRequired();
        builder.Property(c => c.StartDate).IsRequired();
        builder.Property(c => c.EndDate).IsRequired();
        builder.Property(c => c.SupportYears).IsRequired();

        builder.HasOne(c => c.Software)
            .WithMany()
            .HasForeignKey(c => c.SoftwareId);

        builder.HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.CustomerId);
    }
}
