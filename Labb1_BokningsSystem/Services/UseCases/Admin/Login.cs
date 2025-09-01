using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Labb1_BokningsSystem.Services.UseCases.Auth;

public class Login(RestaurantDbContext context, IConfiguration config) : IUseCase<AdminDtos.LoginAdminDto, Login.Response>
{
    public async Task<Response> ExecuteAsync(AdminDtos.LoginAdminDto request)
    {
        var userAdmin = await context.Admins.FirstOrDefaultAsync(a => a.Email == request.Email);
        if (userAdmin == null)
        {
            return new Response(false, "User not found.", null
            );
        }
        
        bool passwordMatch = BCrypt.Net.BCrypt.Verify(request.Password, userAdmin.PasswordHash);
        if (!passwordMatch)
        {
            return new Response(false, "Invalid password.", null
            );
        }

        var token = GenerateJwtToken(userAdmin);
        
        return new Response(true, "Login successful", token
        );
    }
    
    private string GenerateJwtToken(Admin admin)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(config["Jwt:Key"]);

        var claims = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.Email, admin.Email),
        });
        var tokenDescripter = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddHours(2),
            Issuer = "Labb1_BokningsSystem",
            Audience = "Labb1_BokningsSystem",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescripter);
        
        return tokenHandler.WriteToken(token);
    }
    
    public record Response(bool Success, string Message, string? Token);
}