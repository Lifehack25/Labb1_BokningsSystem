using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class CreateBooking(RestaurantDbContext context) : IUseCase<BookingDtos.CreateBookingDto, CreateBooking.Response>
{

    public async Task<Response> ExecuteAsync(BookingDtos.CreateBookingDto request)
    {
        var bookingStart = request.StartTime;
        var bookingEnd = bookingStart.AddHours(2);

        if (request.TableId > 0)
        {
            var selectedTable = await context.Tables
                .Include(t => t.Bookings)
                .FirstOrDefaultAsync(t => t.Id == request.TableId);

            if (selectedTable == null)
            {
                return new Response(false, "Det valda bordet hittades inte.", null, null);
            }

            if (selectedTable.Capacity < request.NumberOfGuests)
            {
                return new Response(false, "Det valda bordet har inte tillräckligt med platser.", null, null);
            }

            var isTaken = selectedTable.Bookings.Any(b =>
                bookingStart < b.StartTime.AddHours(2) &&
                bookingEnd > b.StartTime);

            if (isTaken)
            {
                return new Response(false, "Det valda bordet är redan bokat vid den tiden.", null, null);
            }

            var newBooking = new Models.Booking
            {
                Name = request.Name,
                Phone = request.Phone,
                TableId = selectedTable.Id,
                StartTime = bookingStart,
                NumberOfGuests = request.NumberOfGuests
            };

            context.Bookings.Add(newBooking);
            await context.SaveChangesAsync();

            return new Response(true, "Booking confirmed", newBooking.TableId, newBooking.StartTime);
        }

        var candidateTables = await context.Tables
            .Include(t => t.Bookings)
            .Where(t => t.Capacity >= request.NumberOfGuests)
            .OrderBy(t => t.Capacity)
            .ToListAsync();

        foreach (var table in candidateTables)
        {
            bool isTaken = table.Bookings.Any(b =>
                bookingStart < b.StartTime.AddHours(2) &&
                bookingEnd > b.StartTime
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

                return new Response(true, "Booking confirmed", newBooking.TableId, newBooking.StartTime);
            }
        }

        return new Response(false, "No tables available for the chosen time and party size.", null, null);
    }

    public record Response(bool Success, string Message, int? TableId, DateTime? StartTime);
}
