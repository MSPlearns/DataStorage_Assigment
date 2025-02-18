using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Data.Context;
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<StatusTypeEntity> StatusTypes { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ProjectEntity> Projects { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //This prevents a customer, product, status, or user from being deleted if the last associated project is deleted.

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Customer)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Product)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.Status)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

