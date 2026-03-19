using Microsoft.EntityFrameworkCore;
using DrinkApi.Models;
using DrinkApi.Data.Interfaces;

namespace DrinkApi.Data;

public class OrderRepository : DrinkApi.Data.Interfaces.IOrderRepository
{
    private readonly AppDbContext _context;

    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAll() =>
        await _context.Orders.ToListAsync();

    public async Task<Order?> GetById(Guid id) =>
        await _context.Orders.FindAsync(id);

    public async Task Add(Order order)
    {
        _context.Orders.Add(order);
        // Do not commit here. UnitOfWork/Service will call SaveChangesAsync.
    }
}
