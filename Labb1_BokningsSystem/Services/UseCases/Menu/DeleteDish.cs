using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Menu;

public class DeleteDish(RestaurantDbContext context) : IUseCase<int, DeleteDish.Response>
{
    public async Task<Response> ExecuteAsync(int dishId)
    {
        var dish = await context.Dishes.FirstOrDefaultAsync(d => d.Id == dishId);
        if (dish == null)
        {
            return new Response(false, "Dish not found");
        }

        context.Dishes.Remove(dish);
        await context.SaveChangesAsync();

        return new Response(true, "Dish deleted successfully");
    }

    public record Response(bool Success, string Message);
}