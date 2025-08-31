using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Menu;

public class UpdateDish(RestaurantDbContext context) : IUseCase<DishDtos.UpdateMenuDto, UpdateDish.Response>
{
    public async Task<Response> ExecuteAsync(DishDtos.UpdateMenuDto request)
    {
        var dish = await context.Dishes.FirstOrDefaultAsync(d => d.Id == request.Id);
        if (dish == null)
        {
            return new Response(false, "Dish not found");
        }

        if (!string.IsNullOrEmpty(request.Name))
            dish.Name = request.Name;
        
        if (!string.IsNullOrEmpty(request.Description))
            dish.Description = request.Description;
        
        if (request.Price.HasValue)
            dish.Price = request.Price.Value;
        
        if (request.IsPopular.HasValue)
            dish.IsPopular = request.IsPopular.Value;
        
        if (!string.IsNullOrEmpty(request.ImageUrl))
            dish.ImageUrl = request.ImageUrl;

        await context.SaveChangesAsync();
        
        return new Response(true, "Dish updated successfully"
        );
    }
    
    public record Response(bool Success, string Message);
}