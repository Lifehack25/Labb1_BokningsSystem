using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Table;

public class DeleteTable(RestaurantDbContext context) : IUseCase<int, DeleteTable.Response>
{
    public async Task<Response> ExecuteAsync(int tableId)
    {
        var table = await context.Tables
            .Include(t => t.Bookings)
            .FirstOrDefaultAsync(t => t.Id == tableId);
            
        if (table == null)
        {
            return new Response(false, "Table not found");
        }

        if (table.Bookings.Any())
        {
            return new Response(false, "Cannot delete table with existing bookings");
        }

        context.Tables.Remove(table);
        await context.SaveChangesAsync();

        return new Response(true, "Table deleted successfully");
    }

    public record Response(bool Success, string Message);
}