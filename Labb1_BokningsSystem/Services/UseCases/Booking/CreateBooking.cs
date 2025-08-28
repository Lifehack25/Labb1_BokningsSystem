using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class CreateBooking(RestaurantDbContext _context) : IUseCase<CreateBooking.Request, CreateBooking.Response>
{
    private readonly RestaurantDbContext context = _context;

    public async Task<Response> ExecuteAsync(Request request)
    {
        var bookingStart = request.StartTime;

        var candidateTables = await _context.Tables
            .Include(t => t.Bookings)
            .Where(t => t.Capacity >= request.NumberOfGuests)
            .OrderBy(t => t.Capacity)
            .ToListAsync();

        foreach (var table in candidateTables)
        {
            bool isTaken = table.Bookings.Any(b =>
                bookingStart < b.StartTime.AddHours(2) &&
                bookingStart.AddHours(2) > b.StartTime
            );

            if (!isTaken)
            {
                var newBooking = new Models.Booking
                {
                    Name = request.Name,
                    Phone = request.Phone,
                    TableId = table.Id,
                    StartTime = bookingStart,
                    NumberOfGuests = request.NumberOfGuests
                };

                _context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();

                return new Response(
                    Success: true,
                    Message: "Booking confirmed",
                    TableNumber: newBooking.TableId,
                    StartTime: bookingStart
                );
            }
        }

        return new Response(
            Success: false,
            Message: "No tables available for the chosen time and party size.",
            TableNumber: null,
            StartTime: null
        );
    }

    // Nested request/response
    public record Request(string Name, int Phone, DateTime StartTime, int NumberOfGuests);

    public record Response(bool Success, string Message, int? TableNumber, DateTime? StartTime);
}