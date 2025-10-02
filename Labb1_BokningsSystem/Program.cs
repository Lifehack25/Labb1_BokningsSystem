using System.Text;
using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Services;
using Labb1_BokningsSystem.Services.UseCases.Auth;
using Labb1_BokningsSystem.Services.UseCases.Booking;
using Labb1_BokningsSystem.Services.UseCases.Menu;
using Labb1_BokningsSystem.Services.UseCases.Table;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Labb1_BokningsSystem;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Authentication & Authorization.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
        builder.Services.AddAuthorization();

        // Swagger.
        builder.Services.AddSwaggerGen();
        
        // Controllers.
        builder.Services.AddControllers();
        
        // Database
        builder.Services.AddDbContext<RestaurantDbContext>(options => options.UseSqlite("Data Source=Database.db"));
        
        // Use Cases - Auth
        builder.Services.AddScoped<Register>();
        builder.Services.AddScoped<Login>();
        builder.Services.AddScoped<UpdateAdmin>();
        builder.Services.AddScoped<DeleteAdmin>();
        
        // Use Cases - Booking
        builder.Services.AddScoped<CheckAvailability>();
        builder.Services.AddScoped<CreateBooking>();
        builder.Services.AddScoped<UpdateBooking>();
        builder.Services.AddScoped<DeleteBooking>();
        builder.Services.AddScoped<GetBookings>();
        
        // Use Cases - Menu
        builder.Services.AddScoped<GetMenu>();
        builder.Services.AddScoped<CreateDish>();
        builder.Services.AddScoped<UpdateDish>();
        builder.Services.AddScoped<DeleteDish>();
        
        // Use Cases - Table
        builder.Services.AddScoped<CreateTable>();
        builder.Services.AddScoped<UpdateTable>();
        builder.Services.AddScoped<DeleteTable>();
        builder.Services.AddScoped<GetTables>();
        
        // Services
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IMenuService, MenuService>();
        builder.Services.AddScoped<ITableService, TableService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}