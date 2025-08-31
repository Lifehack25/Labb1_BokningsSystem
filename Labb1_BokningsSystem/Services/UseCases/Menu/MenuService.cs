using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Menu;

namespace Labb1_BokningsSystem.Services;

public class MenuService(GetMenu getMenu, CreateDish createDish, UpdateDish updateDish, DeleteDish deleteDish) : IMenuService
{
    public async Task<GetMenu.Response> GetMenuAsync(DishDtos.GetMenuDto request)
        => await getMenu.ExecuteAsync(request);

    public async Task<CreateDish.Response> CreateDishAsync(DishDtos.CreateDishDto request)
        => await createDish.ExecuteAsync(request);

    public async Task<UpdateDish.Response> UpdateDishAsync(DishDtos.UpdateMenuDto request)
        => await updateDish.ExecuteAsync(request);

    public async Task<DeleteDish.Response> DeleteDishAsync(int dishId)
        => await deleteDish.ExecuteAsync(dishId);
}