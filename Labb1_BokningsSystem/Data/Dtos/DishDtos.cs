namespace Labb1_BokningsSystem.Data.Dtos;

public static class DishDtos
{
    public record GetMenuDto();
    
    public record CreateDishDto(string Name, string Description, decimal Price, bool IsPopular, string? ImageUrl);
    
    public record UpdateMenuRequestDto(int Id, string? Name, string? Description, decimal? Price, bool? IsPopular, string? ImageUrl);
    
    public record UpdateMenuDto(int Id, string? Name, string? Description, decimal? Price, bool? IsPopular, string? ImageUrl);
}