using DrinkApi.Models;

namespace DrinkApi.Data.Interfaces;

public interface IDrinkRepository
{
    Task<List<Drink>> GetAll();
    Task<Drink?> GetById(int id);
    Task Add(Drink drink);
    Task<bool> Update(Drink drink);
    Task<bool> Delete(int id);
}
