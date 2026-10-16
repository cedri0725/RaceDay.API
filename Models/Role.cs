using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table ("Roles")]
    public class Role
    {
        [Key]
        [Column("Role_ID")]
        public int Role_ID { get; set; }

        [Required]
        [Column("Role_Name")]
        [StringLength(50)]
        public string Role_Name { get; set; } = string.Empty;

        [Required]
        [Column("Description")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User> ();
    }
}
