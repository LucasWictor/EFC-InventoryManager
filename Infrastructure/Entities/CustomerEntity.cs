
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities
{
    public class CustomerEntity
    {
        [Key] 
        public int CustomerId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string StreetName { get; set; }
        [Required]
        [StringLength(6)]
        public string PostalCode { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();




    }
}
