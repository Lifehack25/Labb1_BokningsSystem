namespace Labb1_BokningsSystem.Data.Dtos;

public record BookingDto(string Name, int Phone, DateTime StartTime, int NumberOfGuests, int TableId);