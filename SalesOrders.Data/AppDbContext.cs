using Microsoft.EntityFrameworkCore;
using SalesOrders.Domains.Models;


namespace SalesOrders.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesOrder>().HasData(GetSalesOrdersData());
            modelBuilder.Entity<Product>().HasData(GetProductsData());
            modelBuilder.Entity<SalesOrder>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("REAL");
            });

        }
        public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users => Set<User>();
        private static IEnumerable<SalesOrder> GetSalesOrdersData()
        {
            return new List<SalesOrder>()
            {
                new SalesOrder { Id = 1, ProductId = 1, CustomerId = 1, Quantity = 5 },
                new SalesOrder { Id = 2, ProductId = 2, CustomerId = 2, Quantity = 3 }
            };
        }

        private static IEnumerable<Product> GetProductsData()
        {
            return new List<Product>()
            {
                new Product { Id = 1, Name = "Cocacola", Price = 10 },
                new Product { Id = 2, Name = "Pepsi", Price = 20 }
            };
        }
    }
}
