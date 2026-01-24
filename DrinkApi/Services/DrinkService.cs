using DrinkApi.Data;
using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Services;

public class DrinkService : IDrinkService
{
    private readonly DrinkDbContextService _db;

    public DrinkService(DrinkDbContextService db)
    {
        _db = db;
    }

    public async Task<List<DrinkReadDto>> GetAll()
    {
        var drinks = await _db.GetAll();
        return drinks.Select(d => new DrinkReadDto
        {
            Id = d.Id,
            Name = d.Name,
            Type = d.Type,
            Sweetness = d.Sweetness,
            Price = d.Price
        }).ToList();
    }

    public async Task<DrinkReadDto?> GetById(int id)
    {
        var drink = await _db.GetById(id);
        if (drink == null) return null;

        return new DrinkReadDto
        {
            Id = drink.Id,
            Name = drink.Name,
            Type = drink.Type,
            Sweetness = drink.Sweetness,
            Price = drink.Price
        };
    }

    public async Task<DrinkReadDto> Create(DrinkCreateDto dto)
    {
        var drink = new Drink
        {
            Name = dto.Name,
            Type = dto.Type,
            Sweetness = dto.Sweetness,
            Price = dto.Price
        };

        await _db.Add(drink);

        return new DrinkReadDto
        {
            Id = drink.Id,
            Name = drink.Name,
            Type = drink.Type,
            Sweetness = drink.Sweetness,
            Price = drink.Price
        };
    }

    public async Task<bool> Update(int id, DrinkUpdateDto dto)
    {
        var drink = await _db.GetById(id);
        if (drink == null) return false;

        drink.Name = dto.Name;
        drink.Type = dto.Type;
        drink.Sweetness = dto.Sweetness;
        drink.Price = dto.Price;

        return await _db.Update(drink);
    }

    public async Task<bool> Delete(DrinkDeleteDto dto)
    {
        return await _db.Delete(dto.Id);
    }
}
