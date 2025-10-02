using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class UpdateBooking(RestaurantDbContext context) : IUseCase<BookingDtos.UpdateBookingDto, UpdateBooking.Response>
{
    public async Task<Response> ExecuteAsync(BookingDtos.UpdateBookingDto request)
    {
        var booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == request.Id);
        if (booking == null)
        {
            return new Response(false, "Booking not found");
        }

        if (!string.IsNullOrEmpty(request.Name))
            booking.Name = request.Name;

        if (!string.IsNullOrWhiteSpace(request.Phone))
            booking.Phone = request.Phone;

        if (request.StartTime.HasValue)
        {
            var newStartTime = request.StartTime.Value;
            
            var candidateTables = await context.Tables
                .Include(t => t.Bookings)
                .Where(t => t.Capacity >= request.NumberOfGuests)
                .OrderBy(t => t.Capacity)
                .ToListAsync();

            bool canUpdateTime = false;
            int assignedTableId = booking.TableId;

            foreach (var table in candidateTables)
            {
                bool isTaken = table.Bookings.Any(b =>
                    b.Id != booking.Id &&
                    newStartTime < b.StartTime.AddHours(2) &&
                    newStartTime.AddHours(2) > b.StartTime
                );

                if (!isTaken)
                {
                    assignedTableId = table.Id;
                    canUpdateTime = true;
                    break;
                }
            }

            if (!canUpdateTime)
            {
                return new Response(false, "No tables available for the requested time and party size");
            }

            booking.StartTime = newStartTime;
            booking.TableId = assignedTableId;
        }

        if (request.NumberOfGuests.HasValue)
        {
            var newGuestCount = request.NumberOfGuests.Value;
            
            var candidateTables = await context.Tables
                .Include(t => t.Bookings)
                .Where(t => t.Capacity >= newGuestCount)
                .OrderBy(t => t.Capacity)
                .ToListAsync();

            bool canUpdateGuests = false;
            int assignedTableId = booking.TableId;

            foreach (var table in candidateTables)
            {
                bool isTaken = table.Bookings.Any(b =>
                    b.Id != booking.Id &&
                    booking.StartTime < b.StartTime.AddHours(2) &&
                    booking.StartTime.AddHours(2) > b.StartTime
                );

                if (!isTaken)
                {
                    assignedTableId = table.Id;
                    canUpdateGuests = true;
                    break;
                }
            }

            if (!canUpdateGuests)
            {
                return new Response(false, "No tables available that can accommodate the new party size");
            }

            booking.NumberOfGuests = newGuestCount;
            booking.TableId = assignedTableId;
        }

        await context.SaveChangesAsync();
        
        return new Response(true, "Booking updated successfully");
    }
    
    public record Response(bool Success, string Message);
}