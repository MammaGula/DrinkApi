using System;
using System.Threading.Tasks;
using DrinkApi.Data;
using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services;
using DrinkApi.Services.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DrinkApi.Tests
{
    public class OrderServiceTests
    {
        private AppDbContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            var context = new AppDbContext(options);
            return context;
        }

        [Fact]
        public async Task Create_ShouldReturnOrderResponseAndPersist_WhenDrinkExists()
        {
            // Arrange
            var context = CreateInMemoryContext("CreateOrder_TestDB");
            // seed a drink
            context.Drinks.Add(new Drink { Id = 1, Name = "TestDrink", Type = "Tea", Sweetness = 5, Price = 10m });
            await context.SaveChangesAsync();

            var drinkRepo = new DrinkRepository(context);
            var orderRepo = new OrderRepository(context);
            var drinkService = new DrinkService(drinkRepo);
            var orderService = new OrderService(orderRepo, drinkService);

            var dto = new OrderRequestDto { DrinkName = "TestDrink", Quantity = 2 };

            // Act
            var result = await orderService.Create(dto);

            // Assert
            result.Should().NotBeNull();
            result!.DrinkName.Should().Be("TestDrink");
            result.Quantity.Should().Be(2);
            result.UnitPrice.Should().Be(10m);
            result.TotalPrice.Should().Be(20m);

            var saved = await context.Orders.FindAsync(result.Id);
            saved.Should().NotBeNull();
            saved!.TotalPrice.Should().Be(20m);
        }

        [Fact]
        public async Task Create_ShouldReturnNull_WhenDrinkDoesNotExist()
        {
            // Arrange
            var context = CreateInMemoryContext("CreateOrder_NoDrink_TestDB");
            var drinkRepo = new DrinkRepository(context);
            var orderRepo = new OrderRepository(context);
            var drinkService = new DrinkService(drinkRepo);
            var orderService = new OrderService(orderRepo, drinkService);

            var dto = new OrderRequestDto { DrinkName = "NotFound", Quantity = 1 };

            // Act
            var result = await orderService.Create(dto);

            // Assert
            result.Should().BeNull();
        }
    }
}
