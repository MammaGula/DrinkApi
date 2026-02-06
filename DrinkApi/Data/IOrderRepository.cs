using DrinkApi.Models;

namespace DrinkApi.Data;

public interface IOrderRepository
{
    Task<List<Order>> GetAll();
    Task<Order?> GetById(Guid id);
    Task Add(Order order);
}
