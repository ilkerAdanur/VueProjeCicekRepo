using FlowerShop.API.Models;

namespace FlowerShop.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(FlowerShopContext context)
        {
            context.Database.EnsureCreated();

            // Check if database is already seeded
            if (context.Categories.Any())
            {
                return; // Database has been seeded
            }

            // Seed Categories
            var categories = new Category[]
            {
                new Category
                {
                    Name = "Gül",
                    Description = "Sevgi ve tutkunun simgesi güller",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Lale",
                    Description = "Zarafet ve inceliğin simgesi laleler",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Orkide",
                    Description = "Lüks ve şıklığın simgesi orkideler",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Karanfil",
                    Description = "Sadakat ve sevginin simgesi karanfiller",
                    CreatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Name = "Papatya",
                    Description = "Saflık ve masumiyetin simgesi papatyalar",
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // Seed Flowers
            var flowers = new Flower[]
            {
                new Flower
                {
                    Name = "Kırmızı Gül Buketi",
                    Description = "12 adet kırmızı gülden oluşan romantik buket",
                    Price = 150.00m,
                    Stock = 25,
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1518895949257-7621c3c786d7?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Beyaz Gül Buketi",
                    Description = "12 adet beyaz gülden oluşan zarif buket",
                    Price = 140.00m,
                    Stock = 20,
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1563241527-3004b7be0ffd?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Pembe Gül Buketi",
                    Description = "12 adet pembe gülden oluşan sevimli buket",
                    Price = 145.00m,
                    Stock = 30,
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1561181286-d3fee7d55364?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Renkli Lale Buketi",
                    Description = "Karışık renklerde 15 adet lale",
                    Price = 120.00m,
                    Stock = 35,
                    CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1520637836862-4d197d17c93a?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Sarı Lale Buketi",
                    Description = "10 adet sarı laleden oluşan neşeli buket",
                    Price = 110.00m,
                    Stock = 25,
                    CategoryId = 2,
                    ImageUrl = "https://images.unsplash.com/photo-1490750967868-88aa4486c946?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Beyaz Orkide",
                    Description = "Tek dal beyaz orkide, saksıda",
                    Price = 200.00m,
                    Stock = 15,
                    CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1578662996442-48f60103fc96?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Mor Orkide",
                    Description = "Tek dal mor orkide, saksıda",
                    Price = 220.00m,
                    Stock = 12,
                    CategoryId = 3,
                    ImageUrl = "https://images.unsplash.com/photo-1594736797933-d0401ba2fe65?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Kırmızı Karanfil Buketi",
                    Description = "15 adet kırmızı karanfilden oluşan buket",
                    Price = 90.00m,
                    Stock = 40,
                    CategoryId = 4,
                    ImageUrl = "https://images.unsplash.com/photo-1551698618-1dfe5d97d256?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Beyaz Papatya Buketi",
                    Description = "20 adet beyaz papatyadan oluşan doğal buket",
                    Price = 75.00m,
                    Stock = 50,
                    CategoryId = 5,
                    ImageUrl = "https://images.unsplash.com/photo-1574684891174-df6b02ab38d7?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Flower
                {
                    Name = "Karışık Çiçek Buketi",
                    Description = "Farklı çiçek türlerinden oluşan renkli buket",
                    Price = 180.00m,
                    Stock = 20,
                    CategoryId = 1,
                    ImageUrl = "https://images.unsplash.com/photo-1487070183336-b863922373d4?w=400",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Flowers.AddRange(flowers);
            context.SaveChanges();
        }
    }
}
