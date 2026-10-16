using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("User_ID")]
        public int User_ID { get; set; }

        [Required]
        [Column("Role_ID")]
        public int Role_ID { get; set; }

        [Required]
        [Column("Email")]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("Password_Hash")]
        [StringLength(255)]
        public string Password_Hash { get; set; } = string.Empty;

        [Required]
        [Column("Full_Name")]
        [StringLength(100)]
        public string Full_Name { get; set; } = string.Empty;

        [Column("Phone_Number")]
        [StringLength(20)]
        public string? Phone_Number { get; set; }

        [Column("Created_At")]
        public DateTime Created_At { get; set; } = DateTime.Now;

        [Column("Is_Active")]
        public bool Is_Active { get; set; } = true;

        // Navigation properties
        [ForeignKey("Role_ID")]
        public Role? Role { get; set; }

        public ICollection<Event> OrganisedEvents { get; set; } = new List<Event>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}