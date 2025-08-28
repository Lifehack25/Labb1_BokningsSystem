using Labb1_BokningsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Data;

public class RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Dish> Dishes { get; set; }
}