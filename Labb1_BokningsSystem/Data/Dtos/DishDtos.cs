namespace Labb1_BokningsSystem.Data.Dtos;

public static class DishDtos
{
    public record GetMenuDto();
    
    public record CreateDishDto(string Name, string Description, int Price, bool IsPopular, string? ImageUrl);
    
    public record UpdateMenuRequestDto(string? Name, string? Description, int? Price, bool? IsPopular, string? ImageUrl);
    
    public record UpdateMenuDto(int Id, string? Name, string? Description, int? Price, bool? IsPopular, string? ImageUrl);
}