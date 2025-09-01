using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/menu")]
public class MenuController(IMenuService useCase) : ControllerBase
{
    // Fetches a list of all dishes in the database.
    [HttpGet]
    public async Task<IActionResult> GetMenu()
    {
        var response = await useCase.GetMenuAsync(new DishDtos.GetMenuDto());
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Creates a new dish in the menu.
    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateDish(DishDtos.CreateDishDto dto)
    {
        var response = await useCase.CreateDishAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Updates the field of a dish.
    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateMenu(DishDtos.UpdateMenuRequestDto dto)
    {
        var updateDto = new DishDtos.UpdateMenuDto(dto.Id, dto.Name, dto.Description, dto.Price, dto.IsPopular, dto.ImageUrl);
        var response = await useCase.UpdateDishAsync(updateDto);
        
        return response.Success ? Ok(response) : NotFound(response);
    }

    // Deletes a dish from the menu.
    [HttpDelete("delete/{dishId}")]
    [Authorize]
    public async Task<IActionResult> DeleteDish(int dishId)
    {
        var response = await useCase.DeleteDishAsync(dishId);
        return response.Success ? Ok(response) : NotFound(response);
    }
}