using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Auth;

public class DeleteAdmin(RestaurantDbContext context) : IUseCase<int, DeleteAdmin.Response>
{
    public async Task<Response> ExecuteAsync(int adminId)
    {
        var admin = await context.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
        if (admin == null)
        {
            return new Response(false, "Admin not found");
        }

        context.Admins.Remove(admin);
        await context.SaveChangesAsync();

        return new Response(true, "Admin deleted successfully");
    }
    
    public record Response(bool Success, string Message);
}