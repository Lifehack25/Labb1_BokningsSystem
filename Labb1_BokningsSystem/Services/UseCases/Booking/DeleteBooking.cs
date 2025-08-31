using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class DeleteBooking(RestaurantDbContext context) : IUseCase<int, DeleteBooking.Response>
{
    public async Task<Response> ExecuteAsync(int bookingId)
    {
        var booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
            return new Response(false, "Booking not found.");

        context.Bookings.Remove(booking);
        await context.SaveChangesAsync();

        return new Response(true, "Booking deleted successfully.");
    }

    public record Response(bool Success, string Message);
}