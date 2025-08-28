using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class CheckAvailability(RestaurantDbContext _context) : IUseCase<CheckAvailability.Request, CheckAvailability.Response>
{
    private readonly RestaurantDbContext context = _context;

    public async Task<Response> ExecuteAsync(Request request)
    {
        var bookingStart = request.StartTime;
        var bookingEnd = bookingStart.AddHours(2);

        var candidateTables = await context.Tables
            .Include(t => t.Bookings)
            .Where(t => t.Capacity >= request.NumberOfGuests)
            .OrderBy(t => t.Capacity)
            .ToListAsync();

        bool available = candidateTables.Any(table =>
            !table.Bookings.Any(b =>
                bookingStart < b.StartTime.AddHours(2) &&
                bookingEnd > b.StartTime
            ));

        return new Response(available);
    }

    public record Request(DateTime StartTime, int NumberOfGuests);
    public record Response(bool Available);
}