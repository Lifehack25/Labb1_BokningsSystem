using System.ComponentModel.DataAnnotations;

namespace Labb1_BokningsSystem.Models;

public class Table
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int Capacity { get; set; }
    
    public List<Booking> Bookings { get; set; }
}