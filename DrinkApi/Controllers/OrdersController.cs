using Microsoft.AspNetCore.Mvc;
using DrinkApi.DTOs;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IDrinkService _drinkService;

    public OrdersController(IDrinkService drinkService)
    {
        _drinkService = drinkService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto order)
    {
        // Find drink by name
        var drinks = await _drinkService.GetAll();
        var drink = drinks.FirstOrDefault(d => 
            d.Name.Equals(order.DrinkName, StringComparison.OrdinalIgnoreCase));
        
        if (drink == null)
        {
            return NotFound(new { message = $"Drink '{order.DrinkName}' not found" });
        }


        //Simulate order creation 
        return Ok(new 
        { 
            message = "Order created successfully",
            orderId = Guid.NewGuid(),
            drinkName = drink.Name,
            quantity = order.Quantity,
            unitPrice = drink.Price,
            totalPrice = drink.Price * order.Quantity
        });
    }
}
