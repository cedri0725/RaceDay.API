using RaceDay.API.Models;

namespace RaceDay.API.Services
{
    public interface ITokenService
    {
        string CreateToken(User user, string roleName);
    }
}