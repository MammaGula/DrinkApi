using DrinkApi.Data.Interfaces;
using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Services;

public class DrinkService : IDrinkService
{
    private readonly IDrinkRepository _repository;
    private readonly DrinkApi.Data.Interfaces.IUnitOfWork _unitOfWork;

    public DrinkService(IDrinkRepository repository, DrinkApi.Data.Interfaces.IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<List<DrinkReadDto>> GetAll()
    {
        var drinks = await _repository.GetAll();
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
        var drink = await _repository.GetById(id);
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
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var drink = new Drink
        {
            Name = dto.Name,
            Type = dto.Type,
            Sweetness = dto.Sweetness,
            Price = dto.Price
        };

        await _repository.Add(drink);
        await _unitOfWork.SaveChangesAsync();

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
        var drink = await _repository.GetById(id);
        if (drink == null)
            return false;

        drink.Name = dto.Name;
        drink.Type = dto.Type;
        drink.Sweetness = dto.Sweetness;
        drink.Price = dto.Price;

        var ok = await _repository.Update(drink);
        if (!ok) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        if (id <= 0)
            return false;

        var ok = await _repository.Delete(id);
        if (!ok) return false;
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}

































































































