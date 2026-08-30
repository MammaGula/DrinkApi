using DrinkApi.Data.Interfaces;
using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDrinkService _drinkService;
    private readonly DrinkApi.Data.Interfaces.IUnitOfWork _unitOfWork;

    public OrderService(IOrderRepository orderRepository, IDrinkService drinkService, DrinkApi.Data.Interfaces.IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _drinkService = drinkService;
        _unitOfWork = unitOfWork;
    }

// 1. Fetch all orders from the repository and map them to OrderResponseDto objects for returning to the caller
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

    // 2. Fetch a single order by its ID and map it to an OrderResponseDto object for returning to the caller
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

    // 3. Create a new order in the repository and return the created order as an OrderResponseDto
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
        await _unitOfWork.SaveChangesAsync();

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
