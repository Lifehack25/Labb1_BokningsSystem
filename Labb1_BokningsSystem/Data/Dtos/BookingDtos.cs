namespace Labb1_BokningsSystem.Data.Dtos;

public static class BookingDtos
{
    public record CheckAvailabilityDto(DateTime StartTime, int NumberOfGuests);
    
    public record CreateBookingDto(string Name, string Phone, DateTime StartTime, int NumberOfGuests, int TableId);
    
    public record UpdateBookingDto(int Id, string? Name, string? Phone, DateTime? StartTime, int? NumberOfGuests);
}