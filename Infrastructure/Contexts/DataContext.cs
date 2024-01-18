using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<ManufactureEntity> Manufacturers { get; set; }
        public virtual DbSet<OrderDetailEntity> OrderDetails { get; set; }
        public virtual DbSet<OrderEntity> Order { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<ProductEntity>()
                .HasIndex(p => p.Title);

            modelBuilder.Entity<ProductEntity>()
                .HasIndex(p => p.ManufacturerId);
        }
    }
}
