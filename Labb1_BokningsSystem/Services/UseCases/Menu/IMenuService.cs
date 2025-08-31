using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Menu;

namespace Labb1_BokningsSystem.Services;

public interface IMenuService
{
    Task<GetMenu.Response> GetMenuAsync(DishDtos.GetMenuDto request);
    Task<CreateDish.Response> CreateDishAsync(DishDtos.CreateDishDto request);
    Task<UpdateDish.Response> UpdateDishAsync(DishDtos.UpdateMenuDto request);
    Task<DeleteDish.Response> DeleteDishAsync(int dishId);
}