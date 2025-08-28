using System.ComponentModel.DataAnnotations;

namespace Labb1_BokningsSystem.Models;

public class Dish
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public int Price { get; set; }
    
    [Required]
    public bool IsPopular { get; set; } = false;
    
    public string? ImageUrl { get; set; }
}