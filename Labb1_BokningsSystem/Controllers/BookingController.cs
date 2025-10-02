using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/booking")]
public class BookingController(IBookingService useCase) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllBookings()
    {
        var response = await useCase.GetBookingsAsync();
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Checks for available tables based the date, time and number of guests provides by the customer.
    [HttpPost("availability")]
    public async Task<IActionResult> Availability([FromBody] BookingDtos.CheckAvailabilityDto dto)
    {
        var response = await useCase.CheckAvailabilityAsync(dto);
        return Ok(response);
    }

    // Creates a booking on the most appropriate table based on the number of guests.
    [HttpPost("create")]
    public async Task<IActionResult> Book(BookingDtos.CreateBookingDto dto)
    {
        var response = await useCase.CreateBookingAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Updates an existing booking.
    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateBooking(BookingDtos.UpdateBookingDto dto)
    {
        var response = await useCase.UpdateBookingAsync(dto);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Deletes an existing booking.
    [HttpDelete("delete/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> Delete(int bookingId)
    {
        var response = await useCase.DeleteBookingAsync(bookingId);
        return response.Success ? Ok(response) : NotFound(response);
    }
}
