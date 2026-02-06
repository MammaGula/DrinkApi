using DrinkApi.DTOs;
using DrinkApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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

    private static readonly List<OrderResponseDto> _orders = new();

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_orders);
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetOne(Guid id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto order)
    {
        // Find drink by name
        var drinks = await _drinkService.GetAll();
        var drink = drinks.FirstOrDefault(d =>
            d.Name.Equals(order.DrinkName, StringComparison.OrdinalIgnoreCase));

        if (drink == null)
        {
            return NotFound(new { message = $"Drink '{order.DrinkName}' not found" });
        }

        // Create and store the order
        var newOrder = new OrderResponseDto
        {
            Id = Guid.NewGuid(),
            DrinkName = drink.Name,
            Quantity = order.Quantity,
            UnitPrice = drink.Price,
            TotalPrice = drink.Price * order.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        _orders.Add(newOrder);

        // Return 201 Created with location header
        return CreatedAtAction(nameof(GetOne), new { id = newOrder.Id }, newOrder);
    }
}




//OrdersController — Manages orders(Create + Read)

//Endpoints:
//GET /api/Orders — Retrieves all orders(admin use)
//GET /api/Orders/{id} — Retrieves a single order
//POST /api/Orders — Creates an order (Receives OrderRequestDto, Returns OrderResponseDto)
//Who calls: Customer(order.js) sends an order, Admin page (admin.js) retrieves the order for display

//Purpose: Receives the order from the customer, processes it
//(e.g., finds the unit price, calculates the total price, enters created at),
//and stores/returns the result.