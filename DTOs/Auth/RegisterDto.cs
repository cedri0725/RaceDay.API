using System.ComponentModel.DataAnnotations;

namespace RaceDay.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Full_Name { get; set; } = string.Empty;

        public string? Phone_Number { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty; // "Organiser" or "Participant"
    }
}