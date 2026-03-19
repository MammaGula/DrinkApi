using DrinkApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DrinkApi.Data
{
    public class AppDbContext: IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Drink> Drinks { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Drink>()
                .Property(d => d.Price)
                .HasPrecision(10, 2); // 10 digits total, 2 decimals

            modelBuilder.Entity<Order>()
                .Property(o => o.UnitPrice)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(10, 2);

            // Configure relationship between Order and Drink
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Drink)
                .WithMany()
                .HasForeignKey(o => o.DrinkId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Drink>().HasData(
                new Drink { Id = 1, Name = "Latte", Type = "Coffee", Sweetness = 5, Price = 39.00m },
                new Drink { Id = 2, Name = "Green Tea", Type = "Tea", Sweetness = 2, Price = 29.00m },
                new Drink { Id = 3, Name = "Mojito", Type = "Cocktail", Sweetness = 7, Price = 89.00m },
                new Drink { Id = 4, Name = "Espresso", Type = "Coffee", Sweetness = 1, Price = 29.00m },
                new Drink { Id = 5, Name = "Cappuccino", Type = "Coffee", Sweetness = 4, Price = 42.00m },
                new Drink { Id = 6, Name = "Americano", Type = "Coffee", Sweetness = 1, Price = 35.00m },
                new Drink { Id = 7, Name = "Mocha", Type = "Coffee", Sweetness = 7, Price = 45.00m },
                new Drink { Id = 8, Name = "Hot Chocolate", Type = "Chocolate", Sweetness = 9, Price = 38.00m },
                new Drink { Id = 9, Name = "Chai Latte", Type = "Tea", Sweetness = 6, Price = 42.00m },
                new Drink { Id = 10, Name = "Matcha Latte", Type = "Tea", Sweetness = 5, Price = 48.00m },
                new Drink { Id = 11, Name = "Earl Grey", Type = "Tea", Sweetness = 2, Price = 29.00m },
                new Drink { Id = 12, Name = "Peppermint Tea", Type = "Tea", Sweetness = 3, Price = 29.00m },
                new Drink { Id = 13, Name = "Orange Juice", Type = "Juice", Sweetness = 8, Price = 35.00m },
                new Drink { Id = 14, Name = "Apple Juice", Type = "Juice", Sweetness = 7, Price = 35.00m },
                new Drink { Id = 15, Name = "Lemonade", Type = "Juice", Sweetness = 8, Price = 32.00m },
                new Drink { Id = 16, Name = "Smoothie", Type = "Smoothie", Sweetness = 7, Price = 55.00m },
                new Drink { Id = 17, Name = "Milkshake", Type = "Milkshake", Sweetness = 10, Price = 52.00m },
                new Drink { Id = 18, Name = "Cola", Type = "Soda", Sweetness = 9, Price = 25.00m },
                new Drink { Id = 19, Name = "Sprite", Type = "Soda", Sweetness = 8, Price = 25.00m },
                new Drink { Id = 20, Name = "Iced Tea", Type = "Tea", Sweetness = 6, Price = 32.00m },
                new Drink { Id = 21, Name = "Margarita", Type = "Cocktail", Sweetness = 6, Price = 95.00m },
                new Drink { Id = 22, Name = "Piña Colada", Type = "Cocktail", Sweetness = 9, Price = 99.00m },
                new Drink { Id = 23, Name = "Cosmopolitan", Type = "Cocktail", Sweetness = 5, Price = 92.00m },
                new Drink { Id = 24, Name = "Long Island Iced Tea", Type = "Cocktail", Sweetness = 7, Price = 105.00m },
                new Drink { Id = 25, Name = "Water", Type = "Water", Sweetness = 0, Price = 15.00m }
            );
        }
    }
}

