using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myCity.Api.Data;
using myCity.Api.Dtos;
using myCity.Api.Entities;
using myCity.Api.Entities.Enums;

namespace myCity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly MyCityDbContext _context;

        public UsersController(MyCityDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAdminDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserAdminDto
                {
                    Id = u.Id,
                    Mail = u.Mail,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Role = u.Role.ToString()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult<UserAdminDto>> CreateUser([FromBody] CreateUserAdminDto dto)
        {
            if (!Enum.TryParse<UserRole>(dto.Role, true, out var userRole))
            {
                return BadRequest("Niepoprawna rola użytkownika.");
            }

            var existingUser = await _context.Users.AnyAsync(u => u.Mail == dto.Mail);
            if (existingUser)
            {
                return BadRequest("Użytkownik o takim adresie e-mail już istnieje.");
            }

            var user = new User
            {
                Mail = dto.Mail,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = userRole,
                IsActive = true,
                CreationTimestamp = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new UserAdminDto
            {
                Id = user.Id,
                Mail = user.Mail,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString()
            });
        }

        [HttpPut("{id}/role")]
        public async Task<ActionResult<UserAdminDto>> UpdateUserRole(int id, [FromBody] UpdateUserRoleDto dto)
        {
            if (!Enum.TryParse<UserRole>(dto.Role, true, out var userRole))
            {
                return BadRequest("Niepoprawna rola użytkownika.");
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            user.Role = userRole;
            await _context.SaveChangesAsync();

            return Ok(new UserAdminDto
            {
                Id = user.Id,
                Mail = user.Mail,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString()
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception)
            {
                // In case of foreign key constraints, we can disable the user (soft delete)
                user.IsActive = false;
                await _context.SaveChangesAsync();
                return Ok("Użytkownik ma powiązane dane. Został dezaktywowany (dezaktywacja konta).");
            }
        }
    }
}
