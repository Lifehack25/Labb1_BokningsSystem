using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class CreateBooking(RestaurantDbContext context) : IUseCase<BookingDtos.CreateBookingDto, CreateBooking.Response>
{

    public async Task<Response> ExecuteAsync(BookingDtos.CreateBookingDto request)
    {
        var bookingStart = request.StartTime;

        var candidateTables = await context.Tables
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

                context.Bookings.Add(newBooking);
                await context.SaveChangesAsync();

                return new Response(true, $"Booking confirmed, table ID: {newBooking.TableId}, start time is: {newBooking.StartTime}");
            }
        }

        return new Response(false, "No tables available for the chosen time and party size.");
    }

    public record Response(bool Success, string Message);
}