using DrinkApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DrinkApi.Data
{
    public class AppDbContext: DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Drink> Drinks { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Drink>()
                .Property(d => d.Price)
                .HasPrecision(10, 2); // 10 digits total, 2 decimals

            modelBuilder.Entity<Drink>().HasData(
                new Drink { Id = 1, Name = "Latte", Type = "Coffee", Sweetness = 5, Price = 39.00m },
                new Drink { Id = 2, Name = "Green Tea", Type = "Tea", Sweetness = 2, Price = 29.00m },
                new Drink { Id = 3, Name = "Mojito", Type = "Cocktail", Sweetness = 7, Price = 89.00m }
            );
        }



    }
}
