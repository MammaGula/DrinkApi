using DrinkApi.DTOs;

namespace DrinkApi.Services.Interfaces;

public interface IDrinkService
{
    Task<List<DrinkReadDto>> GetAll();
    Task<DrinkReadDto?> GetById(int id);
    Task<DrinkReadDto> Create(DrinkCreateDto dto);
    Task<bool> Update(int id, DrinkUpdateDto dto);
    Task<bool> Delete(int id);
}
