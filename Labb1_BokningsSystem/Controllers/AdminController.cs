using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Labb1_BokningsSystem.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AdminController(RestaurantDbContext _context, IConfiguration _config) : ControllerBase
    {
        private readonly RestaurantDbContext context = _context;
        private readonly IConfiguration config = _config;

        [HttpPost("register")]
        public async Task<IActionResult> Register(AdminRegisterDto newAdmin)
        {
            var existingUser = await context.Admins.FirstOrDefaultAsync(a => a.Email == newAdmin.Email);
            if (existingUser != null)
            {
                return BadRequest("Email is already in use.");
            }
            
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(newAdmin.Password);

            var newAccount = new Admin
            {
                Name = newAdmin.Name,
                Email = newAdmin.Email,
                PasswordHash = passwordHash,
            };
            
            context.Add(newAccount);
            await context.SaveChangesAsync();
            
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginAdminDto loginAdmin)
        {
            var userAdmin = context.Admins.FirstOrDefault(a => a.Email == loginAdmin.Email);
            if (userAdmin == null)
            {
                return Unauthorized("User not found.");
            }
            
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(loginAdmin.Password, userAdmin.PasswordHash);
            if (!passwordMatch)
            {
                return Unauthorized("Invalid password.");
            }

            var token = GenerateJwtToken(userAdmin);
            
            return Ok(new { token });
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
    }
}