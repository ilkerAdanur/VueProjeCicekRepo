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
        public DbSet<Occasion> Occasions { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
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

            // Occasion configuration
            modelBuilder.Entity<Occasion>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Flower configuration
            modelBuilder.Entity<Flower>(entity =>
            {
                entity.HasOne(f => f.Category)
                      .WithMany(c => c.Flowers)
                      .HasForeignKey(f => f.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(f => f.Occasion)
                      .WithMany(o => o.Flowers)
                      .HasForeignKey(f => f.OccasionId)
                      .OnDelete(DeleteBehavior.SetNull);

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

            // Seed Occasions
            modelBuilder.Entity<Occasion>().HasData(
                new Occasion { Id = 1, Name = "Doğum Günü", Description = "Doğum günü kutlamaları için", Icon = "bi-gift", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 2, Name = "Düğün", Description = "Düğün törenleri için", Icon = "bi-heart", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 3, Name = "Yıldönümü", Description = "Yıldönümü kutlamaları için", Icon = "bi-calendar-heart", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 4, Name = "Sevgililer Günü", Description = "14 Şubat sevgililer günü için", Icon = "bi-heart-fill", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 5, Name = "Anneler Günü", Description = "Anneler günü için", Icon = "bi-flower1", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 6, Name = "Babalar Günü", Description = "Babalar günü için", Icon = "bi-person-heart", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 7, Name = "Yeni İş Tebriği", Description = "Yeni işe başlama tebriği", Icon = "bi-briefcase", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 8, Name = "Geçmiş Olsun", Description = "Hastalık durumlarında", Icon = "bi-heart-pulse", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 9, Name = "Teşekkür", Description = "Teşekkür etmek için", Icon = "bi-hand-thumbs-up", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 10, Name = "Başsağlığı", Description = "Taziye için", Icon = "bi-flower2", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 11, Name = "Yeni Bebek", Description = "Bebek doğumu tebriği", Icon = "bi-baby", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 12, Name = "Mezuniyet", Description = "Mezuniyet kutlaması", Icon = "bi-mortarboard", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 13, Name = "Terfi", Description = "Terfi tebriği", Icon = "bi-trophy", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 14, Name = "Nişan", Description = "Nişan töreni için", Icon = "bi-gem", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 15, Name = "Evlilik Teklifi", Description = "Evlilik teklifi için", Icon = "bi-ring", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 16, Name = "Özür Dileme", Description = "Özür dilemek için", Icon = "bi-emoji-frown", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 17, Name = "Karne Hediyesi", Description = "Başarılı karne için", Icon = "bi-journal-check", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 18, Name = "Yeni Ev", Description = "Yeni eve taşınma", Icon = "bi-house-heart", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 19, Name = "Bayram", Description = "Bayram kutlamaları", Icon = "bi-star", IsActive = true, CreatedAt = seedDate },
                new Occasion { Id = 20, Name = "Kutlama", Description = "Genel kutlamalar", Icon = "bi-balloon", IsActive = true, CreatedAt = seedDate }
            );

            // Seed Flowers with creative names
            modelBuilder.Entity<Flower>().HasData(
                // Sevgililer Günü Buketleri
                new Flower { Id = 1, Name = "Aşk-ı Memnu", Description = "12 kırmızı gül ile hazırlanmış tutkulu buket", Price = 185.00m, Stock = 50, CategoryId = 4, OccasionId = 4, ImageUrl = "/images/ask-i-memnu.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 2, Name = "Kalp Hırsızı", Description = "Pembe ve beyaz güller ile romantik buket", Price = 165.00m, Stock = 40, CategoryId = 4, OccasionId = 4, ImageUrl = "/images/kalp-hirsizi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Düğün Buketleri
                new Flower { Id = 3, Name = "Ebedi Mutluluk", Description = "Beyaz güller ve lilyumlarla gelin buketi", Price = 285.00m, Stock = 25, CategoryId = 4, OccasionId = 2, ImageUrl = "/images/ebedi-mutluluk.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 4, Name = "Sonsuz Aşk", Description = "Krem güller ve baby breath ile düğün buketi", Price = 245.00m, Stock = 30, CategoryId = 4, OccasionId = 2, ImageUrl = "/images/sonsuz-ask.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Doğum Günü Buketleri
                new Flower { Id = 5, Name = "Neşe Patlaması", Description = "Renkli gerbera ve güller ile neşeli buket", Price = 125.00m, Stock = 60, CategoryId = 4, OccasionId = 1, ImageUrl = "/images/nese-patlamasi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 6, Name = "Mutluluk Dansı", Description = "Sarı ve turuncu çiçeklerle enerjik buket", Price = 135.00m, Stock = 45, CategoryId = 4, OccasionId = 1, ImageUrl = "/images/mutluluk-dansi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Anneler Günü Buketleri
                new Flower { Id = 7, Name = "Anne Sevgisi", Description = "Pembe güller ve karanfiller ile özel buket", Price = 155.00m, Stock = 55, CategoryId = 4, OccasionId = 5, ImageUrl = "/images/anne-sevgisi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 8, Name = "Şefkat Eli", Description = "Beyaz ve pembe çiçeklerle zarif buket", Price = 145.00m, Stock = 50, CategoryId = 4, OccasionId = 5, ImageUrl = "/images/sefkat-eli.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Geçmiş Olsun Buketleri
                new Flower { Id = 9, Name = "Şifa Dileği", Description = "Beyaz lilyum ve papatyalarla iyileşme buketi", Price = 95.00m, Stock = 35, CategoryId = 4, OccasionId = 8, ImageUrl = "/images/sifa-dilegi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 10, Name = "Umut Işığı", Description = "Sarı çiçeklerle moral verici buket", Price = 105.00m, Stock = 40, CategoryId = 4, OccasionId = 8, ImageUrl = "/images/umut-isigi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Başsağlığı Buketleri
                new Flower { Id = 11, Name = "Huzur Bahçesi", Description = "Beyaz çiçeklerle saygı buketi", Price = 175.00m, Stock = 20, CategoryId = 4, OccasionId = 10, ImageUrl = "/images/huzur-bahcesi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 12, Name = "Sessiz Dua", Description = "Beyaz güller ve lilyumlarla taziye buketi", Price = 195.00m, Stock = 15, CategoryId = 4, OccasionId = 10, ImageUrl = "/images/sessiz-dua.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Yeni İş Tebriği
                new Flower { Id = 13, Name = "Başarı Yolculuğu", Description = "Sarı güller ve orkidelerle tebrik buketi", Price = 165.00m, Stock = 30, CategoryId = 4, OccasionId = 7, ImageUrl = "/images/basari-yolculugu.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 14, Name = "Zirve Tırmanışı", Description = "Turuncu ve kırmızı çiçeklerle motivasyon buketi", Price = 155.00m, Stock = 35, CategoryId = 4, OccasionId = 7, ImageUrl = "/images/zirve-tirmanisi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Yeni Bebek Tebriği
                new Flower { Id = 15, Name = "Minik Melek", Description = "Pembe ve beyaz çiçeklerle bebek buketi", Price = 125.00m, Stock = 40, CategoryId = 4, OccasionId = 11, ImageUrl = "/images/minik-melek.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 16, Name = "Yeni Hayat", Description = "Pastel renkli çiçeklerle hoş geldin buketi", Price = 135.00m, Stock = 35, CategoryId = 4, OccasionId = 11, ImageUrl = "/images/yeni-hayat.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Mezuniyet Buketleri
                new Flower { Id = 17, Name = "Diploma Sevinci", Description = "Mavi ve beyaz çiçeklerle mezuniyet buketi", Price = 145.00m, Stock = 25, CategoryId = 4, OccasionId = 12, ImageUrl = "/images/diploma-sevinci.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 18, Name = "Bilgi Ağacı", Description = "Yeşil ve beyaz çiçeklerle akademik başarı buketi", Price = 155.00m, Stock = 20, CategoryId = 4, OccasionId = 12, ImageUrl = "/images/bilgi-agaci.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },

                // Özür Dileme Buketleri
                new Flower { Id = 19, Name = "Pişmanlık Çiçeği", Description = "Beyaz güller ve mavi çiçeklerle özür buketi", Price = 115.00m, Stock = 30, CategoryId = 4, OccasionId = 16, ImageUrl = "/images/pismanlik-cicegi.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate },
                new Flower { Id = 20, Name = "Barış Dalı", Description = "Beyaz ve yeşil çiçeklerle barışma buketi", Price = 125.00m, Stock = 25, CategoryId = 4, OccasionId = 16, ImageUrl = "/images/baris-dali.jpg", IsActive = true, CreatedAt = seedDate, UpdatedAt = seedDate }
            );

            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@flowershop.com",
                    FirstName = "Admin",
                    LastName = "User",
                    PasswordHash = "AEOEmypQRSZKzj5zc2bT4AQ9opGggat+5A6dcT+Kgtw=", // password: admin123
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = seedDate
                },
                new User
                {
                    Id = 2,
                    Username = "demo",
                    Email = "demo@flowershop.com",
                    FirstName = "Demo",
                    LastName = "Customer",
                    PasswordHash = "/+q6yUKvv+TaQyV1KgtAcpVA12kno4wKC2ushly3UCg=", // password: demo123
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = seedDate
                }
            );

            // Seed Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "Test",
                    LastName = "Customer",
                    Email = "test@example.com",
                    Phone = "555-0123",
                    CreatedAt = seedDate
                }
            );

            // Seed Orders
            modelBuilder.Entity<Order>().HasData(
                new Order
                {
                    Id = 1,
                    OrderNumber = "ORD-20250711-0001",
                    CustomerId = 1,
                    OrderDate = seedDate,
                    Status = OrderStatus.Pending,
                    TotalAmount = 150.00m,
                    DeliveryAddress = "123 Test Street",
                    DeliveryCity = "Istanbul",
                    DeliveryPostalCode = "34000",
                    DeliveryDate = seedDate.AddDays(1),
                    Notes = "Test sipariş"
                },
                new Order
                {
                    Id = 2,
                    OrderNumber = "ORD-20250711-0002",
                    CustomerId = 1,
                    OrderDate = seedDate,
                    Status = OrderStatus.Confirmed,
                    TotalAmount = 200.00m,
                    DeliveryAddress = "456 Demo Avenue",
                    DeliveryCity = "Ankara",
                    DeliveryPostalCode = "06000",
                    DeliveryDate = seedDate.AddDays(2),
                    Notes = "Demo sipariş"
                }
            );

            // Seed Order Items
            modelBuilder.Entity<OrderItem>().HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    FlowerId = 1, // Assuming first flower exists
                    Quantity = 2,
                    UnitPrice = 75.00m
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 2,
                    FlowerId = 2, // Assuming second flower exists
                    Quantity = 1,
                    UnitPrice = 200.00m
                }
            );
        }
    }
}
