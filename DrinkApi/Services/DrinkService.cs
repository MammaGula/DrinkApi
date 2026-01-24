using AutoMapper;
using DrinkApi.Data;
using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DrinkApi.Services;

public class DrinkService : IDrinkService
{
    private readonly DrinkDbContextService _db;
    private readonly IMapper _mapper;



    public DrinkService(DrinkDbContextService db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;

    }


    //public async Task<List<DrinkReadDto>> GetAll()
    //{
    //    var drinks = await _db.GetAll();
    //    return drinks.Select(d => new DrinkReadDto
    //    {
    //        Id = d.Id,
    //        Name = d.Name,
    //        Type = d.Type,
    //        Sweetness = d.Sweetness,
    //        Price = d.Price
    //    }).ToList();
    //}

    public async Task<List<DrinkReadDto>> GetAll()
    {
        var drinks = await _db.GetAll();
        return _mapper.Map<List<DrinkReadDto>>(drinks);
    }

    //---------------------------------------------------------------------------

    //public async Task<DrinkReadDto?> GetById(int id)
    //{
    //    var drink = await _db.GetById(id);
    //    if (drink == null) return null;

    //    return new DrinkReadDto
    //    {
    //        Id = drink.Id,
    //        Name = drink.Name,
    //        Type = drink.Type,
    //        Sweetness = drink.Sweetness,
    //        Price = drink.Price
    //    };
    //}

    public async Task<DrinkReadDto?> GetById(int id)
    {
        var drink = await _db.GetById(id);
        if (drink == null) return null;

        return _mapper.Map<DrinkReadDto>(drink);
    }

    //---------------------------------------------------------------------------


    //public async Task<DrinkReadDto> Create(DrinkCreateDto dto)
    //{
    //    if (dto == null)
    //        throw new ArgumentNullException(nameof(dto));

    //    var drink = new Drink
    //    {
    //        Name = dto.Name,
    //        Type = dto.Type,
    //        Sweetness = dto.Sweetness,
    //        Price = dto.Price
    //    };

    //    await _db.Add(drink);

    //    return new DrinkReadDto
    //    {
    //        Id = drink.Id,
    //        Name = drink.Name,
    //        Type = drink.Type,
    //        Sweetness = drink.Sweetness,
    //        Price = drink.Price
    //    };
    //}

    public async Task<DrinkReadDto> Create(DrinkCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var drink = _mapper.Map<Drink>(dto);

        await _db.Add(drink);

        return _mapper.Map<DrinkReadDto>(drink);
    }

    //---------------------------------------------------------------------------


    //public async Task<bool> Update(int id, DrinkUpdateDto dto)
    //{
    //    var drink = await _db.GetById(id);
    //    if (drink == null)
    //        throw new KeyNotFoundException($"Drink with ID {id} not found.");

    //    drink.Name = dto.Name;
    //    drink.Type = dto.Type;
    //    drink.Sweetness = dto.Sweetness;
    //    drink.Price = dto.Price;

    //    return await _db.Update(drink);
    //}

    public async Task<bool> Update(int id, DrinkUpdateDto dto)
    {
        var drink = await _db.GetById(id);
        if (drink == null)
            throw new KeyNotFoundException($"Drink with ID {id} not found.");

        _mapper.Map(dto, drink);

        return await _db.Update(drink);
    }

    //---------------------------------------------------------------------------


    //public async Task<bool> Delete(DrinkDeleteDto dto)
    //{
    //    if (dto.Id <= 0)
    //        throw new ArgumentException("Invalid ID.");

    //    return await _db.Delete(dto.Id);
    //}


    public async Task<bool> Delete(DrinkDeleteDto dto)
    {
        if (dto.Id <= 0)
            throw new ArgumentException("Invalid ID.");

        return await _db.Delete(dto.Id);
    }


}




