using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDay.API.Models
{
    [Table("Results")]
    public class Result
    {
        [Key]
        [Column("Result_ID")]
        public int Result_ID { get; set; }

        [Required]
        [Column("Enrolment_ID")]
        public int Enrolment_ID { get; set; }

        [Column("Finish_Time")]
        public TimeSpan? Finish_Time { get; set; }

        [Column("Position")]
        public int? Position { get; set; }

        [Column("Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        [Column("Recorded_At")]
        public DateTime Recorded_At { get; set; } = DateTime.Now;

        // Navigation property
        [ForeignKey("Enrolment_ID")]
        public Enrolment? Enrolment { get; set; }
    }
}