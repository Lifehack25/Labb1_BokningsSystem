using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Booking;

namespace Labb1_BokningsSystem.Services;

public class BookingService(CheckAvailability checkAvailability, CreateBooking createBooking, 
    UpdateBooking updateBooking, DeleteBooking deleteBooking) : IBookingService
{
    public async Task<CheckAvailability.Response> CheckAvailabilityAsync(BookingDtos.CheckAvailabilityDto request)
        => await checkAvailability.ExecuteAsync(request);

    public async Task<CreateBooking.Response> CreateBookingAsync(BookingDtos.CreateBookingDto request)
        => await createBooking.ExecuteAsync(request);

    public async Task<UpdateBooking.Response> UpdateBookingAsync(BookingDtos.UpdateBookingDto request)
        => await updateBooking.ExecuteAsync(request);

    public async Task<DeleteBooking.Response> DeleteBookingAsync(int bookingId)
        => await deleteBooking.ExecuteAsync(bookingId);
}