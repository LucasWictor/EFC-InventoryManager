using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class ManufactureEntity
    {
        [Key]
        public int ManufacturerId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Contactinfo { get; set; }

        public List<ProductEntity> Products { get; set; }
    }
}
