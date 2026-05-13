using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using myCity.Api.Data;
using myCity.Api.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace myCity.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly MyCityDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(MyCityDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Mail == dto.Email);

            
            if (user == null || user.Password != dto.Password)
            {
                return null; 
            }

           
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Mail),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2), 
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            
            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                UserId = user.Id,
                Role = user.Role.ToString()
            };
        }
    }
}