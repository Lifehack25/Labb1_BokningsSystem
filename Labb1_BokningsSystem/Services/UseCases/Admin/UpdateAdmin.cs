using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Auth;

public class UpdateAdmin(RestaurantDbContext context) : IUseCase<AdminDtos.UpdateAdminDto, UpdateAdmin.Response>
{
    public async Task<Response> ExecuteAsync(AdminDtos.UpdateAdminDto request)
    {
        var admin = await context.Admins.FirstOrDefaultAsync(a => a.Id == request.Id);
        if (admin == null)
        {
            return new Response(false, "Admin not found");
        }

        if (!string.IsNullOrEmpty(request.Name))
            admin.Name = request.Name;

        if (!string.IsNullOrEmpty(request.Email))
        {
            var existingAdmin = await context.Admins.FirstOrDefaultAsync(a => a.Email == request.Email && a.Id != request.Id);
            if (existingAdmin != null)
            {
                return new Response(false, "Email is already in use by another admin");
            }
            admin.Email = request.Email;
        }

        if (!string.IsNullOrEmpty(request.Password))
        {
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        await context.SaveChangesAsync();
        
        return new Response(true, "Admin updated successfully");
    }
    
    public record Response(bool Success, string Message);
}