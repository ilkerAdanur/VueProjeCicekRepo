using Microsoft.EntityFrameworkCore;
using FlowerShop.API.Models;

namespace FlowerShop.API.Data
{
    public class FlowerShopContext : DbContext
    {
        public FlowerShopContext(DbContextOptions<FlowerShopContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Flower> Flowers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category configuration
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Flower configuration
            modelBuilder.Entity<Flower>(entity =>
            {
                entity.HasOne(f => f.Category)
                      .WithMany(c => c.Flowers)
                      .HasForeignKey(f => f.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(f => f.Price)
                      .HasPrecision(10, 2);
            });

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Order configuration
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasIndex(e => e.OrderNumber).IsUnique();

                entity.HasOne(o => o.Customer)
                      .WithMany(c => c.Orders)
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(o => o.TotalAmount)
                      .HasPrecision(10, 2);
            });

            // OrderItem configuration
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.Flower)
                      .WithMany(f => f.OrderItems)
                      .HasForeignKey(oi => oi.FlowerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(oi => oi.UnitPrice)
                      .HasPrecision(10, 2);
            });

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Gül", Description = "Çeşitli renklerde güller", IsActive = true, CreatedAt = seedDate },
                new Category { Id = 2, Name = "Lale", Description = "Bahar çiçekleri", IsActive = true, CreatedAt = seedDate },
                new Category { Id = 3, Name = "Orkide", Description = "Egzotik orkideler", IsActive = true, CreatedAt = seedDate },
                new Category { Id = 4, Name = "Buket", Description = "Hazır buketler", IsActive = true, CreatedAt = seedDate },
                new Category { Id = 5, Name = "Saksı Çiçekleri", Description = "Saksıda çiçekler", IsActive = true, CreatedAt = seedDate }
            );

            // Seed Flowers
            modelBuilder.Entity<Flower>().HasData(
                new Flower { Id = 1, Name = "Kırmızı Gül", Description = "Klasik kırmızı gül", Price = 15.00m, Stock = 100, CategoryId = 1, ImageUrl = "/images/red-rose.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 2, Name = "Beyaz Gül", Description = "Zarif beyaz gül", Price = 12.00m, Stock = 80, CategoryId = 1, ImageUrl = "/images/white-rose.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 3, Name = "Sarı Lale", Description = "Canlı sarı lale", Price = 8.00m, Stock = 60, CategoryId = 2, ImageUrl = "/images/yellow-tulip.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 4, Name = "Mor Orkide", Description = "Egzotik mor orkide", Price = 45.00m, Stock = 25, CategoryId = 3, ImageUrl = "/images/purple-orchid.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 5, Name = "Karma Buket", Description = "Karışık çiçek buketi", Price = 35.00m, Stock = 40, CategoryId = 4, ImageUrl = "/images/mixed-bouquet.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate }
            );
        }
    }
}
