using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Events")]
    public class Event
    {
        [Key]
        [Column("Event_ID")]
        public int Event_ID { get; set; }

        [Required]
        [Column("Organiser_ID")]
        public int Organiser_ID { get; set; }

        [Required]
        [Column("Event_Name")]
        [StringLength(100)]
        public string Event_Name { get; set; } = string.Empty;

        [Column("Description")]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column("Event_Date")]
        public DateTime Event_Date { get; set; }

        [Required]
        [Column("Location")]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Column("Max_Capacity")]
        public int Max_Capacity { get; set; }

        [Column("Is_Active")]
        public bool Is_Active { get; set; } = true;

        [Column("Created_At")]
        public DateTime Created_At { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("Organiser_ID")]
        public User? Organiser { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();
    }
}