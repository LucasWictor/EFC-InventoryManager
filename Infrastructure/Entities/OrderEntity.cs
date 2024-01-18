
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Entities
{
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; } 
        public int? CustomerId { get; set; }
        [Required]
        public string Status { get; set; }

        public List<OrderDetailEntity> OrderDetails { get; set; } = new List<OrderDetailEntity>();
        public CustomerEntity Customer { get; set; }
    }
}
