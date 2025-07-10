using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerShop.API.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice => Quantity * UnitPrice;

        // Foreign Keys
        public int OrderId { get; set; }
        public int FlowerId { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; } = null!;
        public virtual Flower Flower { get; set; } = null!;
    }
}
