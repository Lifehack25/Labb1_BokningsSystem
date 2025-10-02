using Labb1_BokningsSystem.Data.Dtos;
using Labb1_BokningsSystem.Services.UseCases.Booking;

namespace Labb1_BokningsSystem.Services;

public interface IBookingService
{
    Task<CheckAvailability.Response> CheckAvailabilityAsync(BookingDtos.CheckAvailabilityDto request);
    Task<CreateBooking.Response> CreateBookingAsync(BookingDtos.CreateBookingDto request);
    Task<UpdateBooking.Response> UpdateBookingAsync(BookingDtos.UpdateBookingDto request);
    Task<DeleteBooking.Response> DeleteBookingAsync(int bookingId);
    Task<GetBookings.Response> GetBookingsAsync();
}
