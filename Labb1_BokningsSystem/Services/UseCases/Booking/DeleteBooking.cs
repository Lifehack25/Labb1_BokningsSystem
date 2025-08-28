using Labb1_BokningsSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Services.UseCases.Booking;

public class DeleteBooking(RestaurantDbContext _context) : IUseCase<DeleteBooking.Request, DeleteBooking.Response>
{
    private readonly RestaurantDbContext context = _context;

    public async Task<Response> ExecuteAsync(Request request)
    {
        var booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == request.BookingId);

        if (booking == null)
            return new Response(false, "Booking not found.");

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();

        return new Response(true, "Booking deleted successfully.");
    }

    public record Request(int BookingId);
    public record Response(bool Success, string Message);
}