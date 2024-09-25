using ClinicApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<TestDOC> TestDOC { get; set; }
    public DbSet<TestDetail> TestDetail { get; set; }
    public DbSet<Items> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>();
		modelBuilder.Entity<TestDOC>();
        modelBuilder.Entity<TestDetail>()
        .HasKey(td => new { td.SNO, td.ItemID });
        modelBuilder.Entity<Items>();
    }
}
