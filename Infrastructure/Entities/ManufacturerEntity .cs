using System.ComponentModel.DataAnnotations;

public class ManufacturerEntity
{
    [Key]
    public int ManufacturerId { get; set; }

    [Required]
    public string? ManufacturerName { get; set; } 

    [Required]
    public string? Address { get; set; } 
    [Required]
    public string? ContactInfo { get; set; } 
    public virtual List<ProductEntity>? Products { get; set; } 
}