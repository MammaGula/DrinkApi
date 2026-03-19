using DrinkApi.DTOs;
using DrinkApi.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DrinkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAll();
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetOne(Guid id)
    {
        var order = await _orderService.GetById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDto order)
    {
        var created = await _orderService.Create(order);
        if (created == null)
            return NotFound(new { message = $"Drink '{order.DrinkName}' not found" });

        return CreatedAtAction(nameof(GetOne), new { id = created.Id }, created);
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