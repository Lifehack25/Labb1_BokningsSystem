using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases;
using Labb1_BokningsSystem.Services.UseCases.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/menu")]
public class MenuController(
    IUseCase<DishDtos.GetMenuDto, GetMenu.Response> getMenu,
    IUseCase<DishDtos.CreateDishDto, CreateDish.Response> createDish,
    IUseCase<DishDtos.UpdateMenuDto, UpdateDish.Response> updateMenu,
    IUseCase<int, DeleteDish.Response> deleteDish
) : ControllerBase
{
    // Fetches a list of all dishes in the database.
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var response = await getMenu.ExecuteAsync(new DishDtos.GetMenuDto());
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Creates a new dish in the menu.
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateDish([FromBody] DishDtos.CreateDishDto dto)
    {
        var response = await createDish.ExecuteAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Updates the field of a dish.
    [HttpPut("update/{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateMenu(int id, [FromBody] DishDtos.UpdateMenuRequestDto dto)
    {
        var updateDto = new DishDtos.UpdateMenuDto(id, dto.Name, dto.Description, dto.Price, dto.IsPopular, dto.ImageUrl);
        var response = await updateMenu.ExecuteAsync(updateDto);
        
        return response.Success ? Ok(response) : NotFound(response);
    }

    // Deletes a dish from the menu.
    [HttpDelete("delete/{dishId}")]
    [Authorize]
    public async Task<IActionResult> DeleteDish(int dishId)
    {
        var response = await deleteDish.ExecuteAsync(dishId);
        return response.Success ? Ok(response) : NotFound(response);
    }
}