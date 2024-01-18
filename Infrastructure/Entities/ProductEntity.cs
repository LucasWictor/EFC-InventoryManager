
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class ProductEntity
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Title { get; set; }

        [Required]
        [Column(TypeName ="money")]
        public decimal Price { get; set; }
        [Required]
        public int QuantityInStock { get; set; }
        [Required]
        [ForeignKey("Manufacturer")]
        public int ManufacturerId { get; set; }
        public string Description { get; set; }

        public virtual ManufactureEntity Manufacturer { get; set; }

    }
}
