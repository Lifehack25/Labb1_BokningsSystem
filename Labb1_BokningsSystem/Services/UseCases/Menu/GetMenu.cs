using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Menu;

public class GetMenu(RestaurantDbContext context) : IUseCase<DishDtos.GetMenuDto, GetMenu.Response>
{
    public async Task<Response> ExecuteAsync(DishDtos.GetMenuDto request)
    {
        var dishes = await context.Dishes.ToListAsync();
        if (dishes.Count == 0)
        {
            return new Response(true, "No dishes found in database.", dishes);
        }
        
        return new Response(true, "Menu retrieved successfully", dishes);
    }
    
    public record Response(bool Success, string Message, List<Dish> Dishes);
}