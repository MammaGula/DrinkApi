using DrinkApi.Data.Interfaces;
using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDrinkService _drinkService;

    public OrderService(IOrderRepository orderRepository, IDrinkService drinkService)
    {
        _orderRepository = orderRepository;
        _drinkService = drinkService;
    }

    public async Task<List<OrderResponseDto>> GetAll()
    {
        var orders = await _orderRepository.GetAll();
        return orders.Select(o => new OrderResponseDto
        {
            Id = o.Id,
            DrinkName = o.DrinkName,
            Quantity = o.Quantity,
            UnitPrice = o.UnitPrice,
            TotalPrice = o.TotalPrice,
            CreatedAt = o.CreatedAt
        }).ToList();
    }

    public async Task<OrderResponseDto?> GetById(Guid id)
    {
        var order = await _orderRepository.GetById(id);
        if (order == null) return null;

        return new OrderResponseDto
        {
            Id = order.Id,
            DrinkName = order.DrinkName,
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice,
            TotalPrice = order.TotalPrice,
            CreatedAt = order.CreatedAt
        };
    }

    public async Task<OrderResponseDto?> Create(OrderRequestDto dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        // Find drink by name using existing drink service
        var drinks = await _drinkService.GetAll();
        var drink = drinks.FirstOrDefault(d => d.Name.Equals(dto.DrinkName, StringComparison.OrdinalIgnoreCase));

        if (drink == null)
            return null; // caller (controller) will return NotFound

        var order = new Order
        {
            Id = Guid.NewGuid(),
            DrinkName = drink.Name,
            Quantity = dto.Quantity,
            UnitPrice = drink.Price,
            TotalPrice = drink.Price * dto.Quantity,
            CreatedAt = DateTime.UtcNow
        };

        await _orderRepository.Add(order);

        return new OrderResponseDto
        {
            Id = order.Id,
            DrinkName = order.DrinkName,
            Quantity = order.Quantity,
            UnitPrice = order.UnitPrice,
            TotalPrice = order.TotalPrice,
            CreatedAt = order.CreatedAt
        };
    }
}
