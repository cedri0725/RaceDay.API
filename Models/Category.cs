using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        [Column("Category_ID")]
        public int Category_ID { get; set; }

        [Required]
        [Column("Category_Name")]
        [StringLength(50)]
        public string Category_Name { get; set; } = string.Empty;

        [Column("Description")]
        [StringLength(200)]
        public string? Description { get; set; }

        [Required]
        [Column("Distance_Km")]
        public decimal Distance_Km { get; set; }

        // Navigation property
        public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
    }
}