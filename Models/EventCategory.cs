using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("EventCategories")]
    public class EventCategory
    {
        [Key]
        [Column("Event_Category_ID")]
        public int Event_Category_ID { get; set; }

        [Required]
        [Column("Event_ID")]
        public int Event_ID { get; set; }

        [Required]
        [Column("Category_ID")]
        public int Category_ID { get; set; }

        [Column("Entry_Fee")]
        public decimal Entry_Fee { get; set; } = 0.00m;

        [Required]
        [Column("Start_Time")]
        public TimeSpan Start_Time { get; set; }

        [Column("Is_Active")]
        public bool Is_Active { get; set; } = true;

        // Navigation properties
        [ForeignKey("Event_ID")]
        public Event? Event { get; set; }

        [ForeignKey("Category_ID")]
        public Category? Category { get; set; }

        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}