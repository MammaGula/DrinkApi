using Microsoft.EntityFrameworkCore;
using DrinkApi.Models;
using DrinkApi.Data.Interfaces;

namespace DrinkApi.Data;

public class DrinkRepository : DrinkApi.Data.Interfaces.IDrinkRepository
{
    private readonly AppDbContext _context;

    public DrinkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Drink>> GetAll() =>
        await _context.Drinks.ToListAsync();

    public async Task<Drink?> GetById(int id) =>
        await _context.Drinks.FindAsync(id);

    public async Task Add(Drink drink)
    {
        _context.Drinks.Add(drink);
        // Do not commit here. UnitOfWork/Service will call SaveChangesAsync.
    }

    public async Task<bool> Update(Drink drink)
    {
        var exists = await _context.Drinks.AnyAsync(d => d.Id == drink.Id);
        if (!exists)
            return false;

        _context.Drinks.Update(drink);
        // Do not commit here. UnitOfWork/Service will call SaveChangesAsync.
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var drink = await _context.Drinks.FindAsync(id);
        if (drink == null) return false;

        _context.Drinks.Remove(drink);
        // Do not commit here. UnitOfWork/Service will call SaveChangesAsync.
        return true;
    }
}