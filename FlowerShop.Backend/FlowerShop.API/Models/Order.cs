using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerShop.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderNumber { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [StringLength(200)]
        public string DeliveryAddress { get; set; } = string.Empty;

        [StringLength(50)]
        public string DeliveryCity { get; set; } = string.Empty;

        [StringLength(10)]
        public string DeliveryPostalCode { get; set; } = string.Empty;

        public DateTime? DeliveryDate { get; set; }

        [StringLength(10)]
        public string DeliveryTime { get; set; } = string.Empty;

        [StringLength(500)]
        public string Notes { get; set; } = string.Empty;

        // Foreign Key
        public int CustomerId { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending = 0,
        Confirmed = 1,
        Preparing = 2,
        Shipped = 3,
        Delivered = 4,
        Cancelled = 5
    }
}
