using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Menu;

public class CreateDish(RestaurantDbContext context) : IUseCase<DishDtos.CreateDishDto, CreateDish.Response>
{
    public async Task<Response> ExecuteAsync(DishDtos.CreateDishDto request)
    {
        var existingDish = await context.Dishes.FirstOrDefaultAsync(d => d.Name == request.Name);
        if (existingDish != null)
        {
            return new Response(false, "A dish with this name already exists", null);
        }

        var newDish = new Dish
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            IsPopular = request.IsPopular,
            ImageUrl = request.ImageUrl
        };

        context.Dishes.Add(newDish);
        await context.SaveChangesAsync();

        return new Response(true, "Dish created successfully", newDish.Id);
    }

    public record Response(bool Success, string Message, int? DishId);
}