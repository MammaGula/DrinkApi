using Microsoft.EntityFrameworkCore;
using DrinkApi.Models;

namespace DrinkApi.Data;

public class DrinkRepository : IDrinkRepository
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
        await _context.SaveChangesAsync(); // EF Core will generate Id to drink
    }

    public async Task<bool> Update(Drink drink)
    {
        var exists = await _context.Drinks.AnyAsync(d => d.Id == drink.Id);
        if (!exists)
            return false;

        _context.Drinks.Update(drink);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var drink = await _context.Drinks.FindAsync(id);
        if (drink == null) return false;

        _context.Drinks.Remove(drink);
        await _context.SaveChangesAsync();
        return true;
    }
}