using Labb1_BokningsSystem.Data;
using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Labb1_BokningsSystem.Controllers;

    [Route("api/booking")]
    [ApiController]

public class BookingController(RestaurantDbContext _context) : ControllerBase
{
    private readonly RestaurantDbContext context = _context;
    
    // Checks for available tables to book based on date, time and number of guests.
    [HttpGet("availability")]
    public async Task<IActionResult> MakeBooking([FromBody] DateTime dateTime, int numberOfGuests)
    {
        var bookingStart = dateTime;
        var bookingEnd = bookingStart.AddHours(2);

        // Find tables that can fit the guests
        var candidateTables = await context.Tables
            .Include(t => t.Bookings)
            .Where(t => t.Capacity >= numberOfGuests)
            .OrderBy(t => t.Capacity)
            .ToListAsync();

        bool available = candidateTables.Any(table =>
            !table.Bookings.Any(b => bookingStart < b.StartTime.AddHours(2) && bookingEnd > b.StartTime)
        );

        return Ok(new { Available = available });
    }

    // Creates a booking.
    [HttpPost("booking")]
    public async Task<IActionResult> Book([FromBody] BookingDto booking)
    {
        var bookingStart = booking.StartTime;

        // Get tables that can fit the guests, smallest first
        var candidateTables = await context.Tables
            .Include(t => t.Bookings)
            .Where(t => t.Capacity >= booking.NumberOfGuests)
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
                var newBooking = new Booking
                {
                    Name = booking.Name,
                    Phone = booking.Phone,
                    TableId = table.Id,
                    StartTime = bookingStart,
                    NumberOfGuests = booking.NumberOfGuests
                };

                context.Bookings.Add(newBooking);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Booking confirmed",
                    TableNumber = newBooking.TableId,
                    StartTime = bookingStart
                });
            }
        }

        return BadRequest(new { Message = "No tables available for the chosen time and party size." });
    }

    // Deletes an booking.
    [HttpGet("delete")]
    public async Task<IActionResult> DeleteBooking(int bookingId)
    {
        Booking booking = await context.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking == null)
        {
            return NotFound(new { Message = "Booking not found." });
        }
        context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        
        return Ok(new { Message = "Booking deleted successfully." });
    }
}