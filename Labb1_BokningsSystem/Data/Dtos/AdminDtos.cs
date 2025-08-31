namespace Labb1_BokningsSystem.Data.Dtos;

public static class AdminDtos
{
    public record AdminRegisterDto(string Name, string Email, string Password);
    
    public record LoginAdminDto(string Email, string Password);
    
    public record UpdateAdminDto(int Id, string? Name, string? Email, string? Password);
}