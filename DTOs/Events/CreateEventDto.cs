using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.DTOs.Events
{
    public class CreateEventDto
    {
        [Required]
        [StringLength(100)]
        public string Event_Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime Event_Date { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Max capacity must be greater than 0")]
        public int Max_Capacity { get; set; }
    }
}