namespace Labb1_BokningsSystem.Data.Dtos;

public static class BookingDtos
{
    // For checking table availability.
    public record CheckAvailabilityDto(DateTime StartTime, int NumberOfGuests);
    
    // For creating new bookings.
    public record CreateBookingDto(string Name, int Phone, DateTime StartTime, int NumberOfGuests, int TableId);
    
    // For updating existing booking.
    public record UpdateBookingDto(int Id, string? Name, int? Phone, DateTime? StartTime, int? NumberOfGuests);
}