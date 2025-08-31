using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/booking")]
public class BookingController(IBookingService bookingService) : ControllerBase
{
    // Checks for available tables based the date, time and number of guests provides by the customer.
    [HttpGet("availability")]
    public async Task<IActionResult> Availability([FromBody] BookingDtos.CheckAvailabilityDto dto)
    {
        var response = await bookingService.CheckAvailabilityAsync(dto);
        return Ok(response);
    }

    // Creates a booking on the most appropriate table based on the number of guests.
    [HttpPost("create")]
    public async Task<IActionResult> Book([FromBody] BookingDtos.CreateBookingDto dto)
    {
        var response = await bookingService.CreateBookingAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Updates an existing booking.
    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateBooking([FromBody] BookingDtos.UpdateBookingDto dto)
    {
        var response = await bookingService.UpdateBookingAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Deletes an existing booking.
    [HttpDelete("delete/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> Delete(int bookingId)
    {
        var response = await bookingService.DeleteBookingAsync(bookingId);
        return response.Success ? Ok(response) : NotFound(response);
    }
}