using Projekt.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Projekt.Models;

namespace Projekt.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<IndividualCustomer> IndividualCustomers { get; set; }
    public DbSet<CompanyCustomer> CompanyCustomers { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Software> Softwares { get; set; }  
    public DbSet<Discount> Discounts { get; set; }  
    public DbSet<Contract> Contracts { get; set; } 
    public DbSet<Payment> Payments { get; set; }  



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new IndividualCustomerConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyCustomerConfiguration());
        modelBuilder.ApplyConfiguration(new SoftwareConfiguration());
        modelBuilder.ApplyConfiguration(new DiscountConfiguration());
        modelBuilder.ApplyConfiguration(new ContractConfiguration());
        modelBuilder.ApplyConfiguration(new PaymentConfiguration());

        modelBuilder.Entity<Customer>()
            .HasDiscriminator<string>("CustomerType")
            .HasValue<IndividualCustomer>("IndividualCustomer")
            .HasValue<CompanyCustomer>("CompanyCustomer");
    }
}


