using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projekt.Models;

namespace Projekt.EfConfiguration;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Address).IsRequired().HasMaxLength(70);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(15);
        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(12);
        if (builder.HasIndex(c => c.IsDeleted) != null) ;
    }
}
