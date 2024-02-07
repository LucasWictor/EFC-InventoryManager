using Infrastructure.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class ProductEntity
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    [Column(TypeName = "money")]
    public decimal Price { get; set; }

    [Required]
    public int QuantityInStock { get; set; }

    public int? ManufacturerId { get; set; } 

    public string? ManufacturerName { get; set; } 

    public string Description { get; set; }

    [ForeignKey("ManufacturerId")]
    public virtual ManufacturerEntity Manufacturer { get; set; }
}