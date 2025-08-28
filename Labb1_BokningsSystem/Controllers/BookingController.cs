using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases;
using Labb1_BokningsSystem.Services.UseCases.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb1_BokningsSystem.Controllers;

[ApiController]
[Route("api/booking")]
public class BookingController(
    IUseCase<CheckAvailability.Request, CheckAvailability.Response> checkAvailability,
    IUseCase<CreateBooking.Request, CreateBooking.Response> createBooking,
    IUseCase<DeleteBooking.Request, DeleteBooking.Response> deleteBooking
) : ControllerBase
{
    // Checks for available tables based the date, time and number of guests provides by the customer.
    [HttpGet("availability")]
    public async Task<IActionResult> Availability([FromQuery] DateTime dateTime, [FromQuery] int numberOfGuests)
    {
        var response = await checkAvailability.ExecuteAsync(
            new CheckAvailability.Request(dateTime, numberOfGuests));
        return Ok(response);
    }

    // Creates a booking on the most appropriate table based on the number of guests.
    [HttpPost("booking")]
    public async Task<IActionResult> Book([FromBody] BookingDto dto)
    {
        var request = new CreateBooking.Request(dto.Name, dto.Phone, dto.StartTime, dto.NumberOfGuests);
        var response = await createBooking.ExecuteAsync(request);

        return response.Success ? Ok(response) : BadRequest(response);
    }

    // Deletes an existing booking.
    [HttpDelete("delete/{bookingId}")]
    [Authorize]
    public async Task<IActionResult> Delete(int bookingId)
    {
        var response = await deleteBooking.ExecuteAsync(new DeleteBooking.Request(bookingId));
        return response.Success ? Ok(response) : NotFound(response);
    }
}