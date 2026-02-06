using DrinkApi.DTOs;
using DrinkApi.Models;
using DrinkApi.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _service.Create(dto);

        // Create HTTP 201, Created response
        return CreatedAtAction(
            nameof(GetOne),           // Name of the action to generate the URL for
            new { id = created.Id },  // route values
            created                    // response body
        );
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DrinkUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var ok = await _service.Update(id, dto);
        if (!ok) return NotFound();
        return NoContent();
    }



    [HttpDelete("{id:int:min(1)}")] // with constraint to ensure id is a positive integer   
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.Delete(id);
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





//DrinksController — Manages the drink menu(CRUD)


//Endpoints:
//GET /api/Drinks — Retrieves all drink entries
//GET /api/Drinks/{id} — Retrieves a single drink
//POST /api/Drinks — Creates a new drink(receives DrinkCreateDto)
//PUT / api / Drinks /{ id} — Updates a drink (receives DrinkUpdateDto)
//DELETE /api/Drinks/{id} — Deletes a drink


//Called by: Admin page(admin.js) — Adds / edits / deletes / loads menu items
//Purpose: Allows the admin to manage menu data, provides backend validation and data storage.