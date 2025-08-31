namespace Labb1_BokningsSystem.Data.Dtos;

public static class TableDtos
{
    public record CreateTableDto(int Capacity);
    
    public record UpdateTableDto(int Id, int? Capacity);
}