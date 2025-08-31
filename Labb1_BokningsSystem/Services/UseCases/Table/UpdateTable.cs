using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Table;

public class UpdateTable(RestaurantDbContext context) : IUseCase<TableDtos.UpdateTableDto, UpdateTable.Response>
{
    public async Task<Response> ExecuteAsync(TableDtos.UpdateTableDto request)
    {
        var table = await context.Tables.FirstOrDefaultAsync(t => t.Id == request.Id);
        if (table == null)
        {
            return new Response(false, "Table not found");
        }

        if (request.Capacity.HasValue)
        {
            table.Capacity = request.Capacity.Value;
        }

        await context.SaveChangesAsync();
        
        return new Response(true, "Table updated successfully");
    }
    
    public record Response(bool Success, string Message);
}