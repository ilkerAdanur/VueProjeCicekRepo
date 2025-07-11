using System.ComponentModel.DataAnnotations;

namespace FlowerShop.API.Models
{
    public class Occasion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [StringLength(50)]
        public string Icon { get; set; } = string.Empty; // Bootstrap icon class

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual ICollection<Flower> Flowers { get; set; } = new List<Flower>();
    }
}
