using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Enrolments")]
    public class Enrolment
    {
        [Key]
        [Column("Enrolment_ID")]
        public int Enrolment_ID { get; set; }

        [Required]
        [Column("Event_Category_ID")]
        public int Event_Category_ID { get; set; }

        [Required]
        [Column("Participant_ID")]
        public int Participant_ID { get; set; }

        [Column("Enrolment_Date")]
        public DateTime Enrolment_Date { get; set; } = DateTime.Now;

        [Column("Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        // Navigation properties
        [ForeignKey("Event_Category_ID")]
        public EventCategory? EventCategory { get; set; }

        [ForeignKey("Participant_ID")]
        public User? Participant { get; set; }

        public Result? Result { get; set; }
    }
}