using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs.Auth;
using RaceDay.API.Models;
using BCrypt.Net;

namespace RaceDay.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly RaceDayDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(RaceDayDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Register a new user account (Organiser or Participant)
        /// </summary>
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            // 1. Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already registered");

            // 2. Find the role
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Role_Name == dto.Role);
            if (role == null)
                throw new Exception("Invalid role. Must be 'Organiser' or 'Participant'");

            // 3. Create the user with a hashed password
            var user = new User
            {
                Email = dto.Email,
                Password_Hash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Full_Name = dto.Full_Name,
                Phone_Number = dto.Phone_Number,
                Role_ID = role.Role_ID,
                Created_At = DateTime.Now,
                Is_Active = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 4. Generate JWT token
            var token = _tokenService.CreateToken(user, role.Role_Name);

            return new AuthResponseDto
            {
                User_ID = user.User_ID,
                Email = user.Email,
                Full_Name = user.Full_Name,
                Role = role.Role_Name,
                Token = token
            };
        }

        /// <summary>
        /// Authenticate a user and return a JWT token
        /// </summary>
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            // 1. Find user by email
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                throw new Exception("Invalid email or password");

            // 2. Verify password
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password_Hash))
                throw new Exception("Invalid email or password");

            // 3. Check if account is active
            if (!user.Is_Active)
                throw new Exception("Account is deactivated");

            // 4. Generate JWT token
            var token = _tokenService.CreateToken(user, user.Role!.Role_Name);

            return new AuthResponseDto
            {
                User_ID = user.User_ID,
                Email = user.Email,
                Full_Name = user.Full_Name,
                Role = user.Role.Role_Name,
                Token = token
            };
        }
    }
}