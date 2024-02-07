using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<ManufacturerEntity> Manufacturers { get; set; }
        public virtual DbSet<OrderDetailEntity> OrderDetails { get; set; }
        public virtual DbSet<OrderEntity> Order { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // set the logging level to Critical
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Critical);
            });

            optionsBuilder
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging(false)
                .EnableDetailedErrors(false);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<ProductEntity>()
                .HasIndex(p => p.Title);

            //modelBuilder.Entity<ProductEntity>()
               //.HasIndex(p => p.ManufacturerName);
        }
    }
}
