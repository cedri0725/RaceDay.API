using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.DTOs.Results
{
    public class CreateResultDto
    {
        [Required]
        public int Enrolment_ID { get; set; }

        public TimeSpan? Finish_Time { get; set; }

        public int? Position { get; set; }
    }
}