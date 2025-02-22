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
            .HasOne(x => x.Status)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProjectEntity>()
            .HasOne(x => x.User)
            .WithMany(x => x.Projects)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        //This is AI generated code to help manage the delete behaviour of the mant-to-many relationship between projects and products
        //
        modelBuilder.Entity<ProjectEntity>()
        .HasMany(x => x.Products)
        .WithMany(x => x.Projects)
        .UsingEntity<Dictionary<string, object>>( //This part manually specifies the join table (which is otherwise automatically done by EF Core) which
            "ProjectProduct",                     // is necessary to configure custom delete behavior in many-to-many relationships                   
            j => j.HasOne<ProductEntity>().WithMany().HasForeignKey("ProductId")
                .OnDelete(DeleteBehavior.Restrict),  //prevents deleting a product that is still associated with any project
            j => j.HasOne<ProjectEntity>().WithMany().HasForeignKey("ProjectId")
                .OnDelete(DeleteBehavior.Cascade)  //deleting a project will delete the associated join table entries, but not the product entity itself
        );
    }
}

