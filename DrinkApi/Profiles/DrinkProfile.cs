using AutoMapper;
using DrinkApi.Models;
using DrinkApi.DTOs;

namespace DrinkApi.Profiles;

public class DrinkProfile : Profile
{
    public DrinkProfile()
    {
        CreateMap<Drink, DrinkReadDto>();
        CreateMap<DrinkCreateDto, Drink>();
        CreateMap<DrinkUpdateDto, Drink>();
        CreateMap<DrinkDeleteDto, Drink>();
    }
}