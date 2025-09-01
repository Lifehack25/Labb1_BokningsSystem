namespace Labb1_BokningsSystem.Data.Dtos;

public static class BookingDtos
{
    public record CheckAvailabilityDto(DateTime StartTime, int NumberOfGuests);
    
    public record CreateBookingDto(string Name, int Phone, DateTime StartTime, int NumberOfGuests, int TableId);
    
    public record UpdateBookingDto(int Id, string? Name, int? Phone, DateTime? StartTime, int? NumberOfGuests);
}