using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;

namespace Labb1_BokningsSystem.Services.UseCases.Table;

public class CreateTable(RestaurantDbContext context) : IUseCase<TableDtos.CreateTableDto, CreateTable.Response>
{
    public async Task<Response> ExecuteAsync(TableDtos.CreateTableDto request)
    {
        if (request.Capacity <= 0)
        {
            return new Response(false, "Capacity must be greater than 0", null);
        }

        var newTable = new Models.Table
        {
            Capacity = request.Capacity,
            Bookings = new List<Models.Booking>()
        };

        context.Tables.Add(newTable);
        await context.SaveChangesAsync();

        return new Response(true, "Table created successfully", newTable.Id);
    }

    public record Response(bool Success, string Message, int? TableId);
}