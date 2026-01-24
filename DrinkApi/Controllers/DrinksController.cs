using Microsoft.AspNetCore.Mvc;
using DrinkApi.DTOs;
using DrinkApi.Services.Interfaces;

namespace DrinkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DrinksController : ControllerBase
{
    private readonly IDrinkService _service;

    public DrinksController(IDrinkService service)
    {
        _service = service;
    }



    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.GetAll());
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(int id)
    {
        var drink = await _service.GetById(id);
        if (drink == null) return NotFound();
        return Ok(drink);
    }



    [HttpPost]
    public async Task<IActionResult> Create(DrinkCreateDto dto)
    {
        var created = await _service.Create(dto);
        return Created("", created);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DrinkUpdateDto dto)
    {
        var ok = await _service.Update(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }



    [HttpDelete]
    public async Task<IActionResult> Delete(DrinkDeleteDto dto)
    {
        var ok = await _service.Delete(dto);
        if (!ok) return NotFound();
        return NoContent();
    }


    // For testing error handling middleware only
    //[HttpGet("test-error")]
    //public IActionResult TestError()
    //{
    //    throw new Exception("This is a test error");
    //}



}
