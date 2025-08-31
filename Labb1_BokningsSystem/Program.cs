using System.Text;
using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases;
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
        builder.Services.AddScoped<IUseCase<AdminDtos.AdminRegisterDto, Register.Response>, Register>();
        builder.Services.AddScoped<IUseCase<AdminDtos.LoginAdminDto, Login.Response>, Login>();
        builder.Services.AddScoped<IUseCase<AdminDtos.UpdateAdminDto, UpdateAdmin.Response>, UpdateAdmin>();
        builder.Services.AddScoped<IUseCase<int, DeleteAdmin.Response>, DeleteAdmin>();
        
        // Use Cases - Booking
        builder.Services.AddScoped<IUseCase<BookingDtos.CheckAvailabilityDto, CheckAvailability.Response>, CheckAvailability>();
        builder.Services.AddScoped<IUseCase<BookingDtos.CreateBookingDto, CreateBooking.Response>, CreateBooking>();
        builder.Services.AddScoped<IUseCase<BookingDtos.UpdateBookingDto, UpdateBooking.Response>, UpdateBooking>();
        builder.Services.AddScoped<IUseCase<int, DeleteBooking.Response>, DeleteBooking>();
        
        // Use Cases - Menu
        builder.Services.AddScoped<IUseCase<DishDtos.GetMenuDto, GetMenu.Response>, GetMenu>();
        builder.Services.AddScoped<IUseCase<DishDtos.CreateDishDto, CreateDish.Response>, CreateDish>();
        builder.Services.AddScoped<IUseCase<DishDtos.UpdateMenuDto, UpdateDish.Response>, UpdateDish>();
        builder.Services.AddScoped<IUseCase<int, DeleteDish.Response>, DeleteDish>();
        
        // Use Cases - Table
        builder.Services.AddScoped<IUseCase<TableDtos.CreateTableDto, CreateTable.Response>, CreateTable>();
        builder.Services.AddScoped<IUseCase<TableDtos.UpdateTableDto, UpdateTable.Response>, UpdateTable>();
        builder.Services.AddScoped<IUseCase<int, DeleteTable.Response>, DeleteTable>();

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