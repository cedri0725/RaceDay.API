namespace RaceDay.API.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int User_ID { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Full_Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}