using DrinkApi.DTOs;

namespace DrinkApi.Services;

public interface IOrderService
{
    Task<List<OrderResponseDto>> GetAll();
    Task<OrderResponseDto?> GetById(Guid id);
    Task<OrderResponseDto?> Create(OrderRequestDto dto);
}
