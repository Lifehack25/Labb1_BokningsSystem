using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class CheckAvailability(RestaurantDbContext context) : IUseCase<BookingDtos.CheckAvailabilityDto, CheckAvailability.Response>
{
    public async Task<Response> ExecuteAsync(BookingDtos.CheckAvailabilityDto request)
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

    public record Response(bool Available);
}