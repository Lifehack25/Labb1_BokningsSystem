using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Labb1_BokningsSystem.Models;

public class Booking
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Phone { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public int NumberOfGuests { get; set; }

    public DateTime CreateAt { get; set; } = DateTime.Now;
   
    [Required]
    public int TableId { get; set; }
    
    [ForeignKey("TableId")]
    public Table Table { get; set; }
}