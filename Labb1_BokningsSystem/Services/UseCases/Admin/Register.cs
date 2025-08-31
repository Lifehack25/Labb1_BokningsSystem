using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Auth;

public class Register(RestaurantDbContext context) : IUseCase<AdminDtos.AdminRegisterDto, Register.Response>
{
    public async Task<Response> ExecuteAsync(AdminDtos.AdminRegisterDto request)
    {
        var existingUser = await context.Admins.FirstOrDefaultAsync(a => a.Email == request.Email);
        if (existingUser != null)
        {
            return new Response(
                Success: false,
                Message: "Email is already in use."
            );
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newAdmin = new Admin
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
        };
        
        context.Add(newAdmin);
        await context.SaveChangesAsync();
        
        return new Response(true, "Registration successful."
        );
    }
    
    public record Response(bool Success, string Message);
}