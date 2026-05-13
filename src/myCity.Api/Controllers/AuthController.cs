using Microsoft.AspNetCore.Mvc;
using myCity.Api.Dtos;
using myCity.Api.Services;


namespace myCity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto);

            if (response == null)
            {
                return Unauthorized("Nieprawidłowy adres email lub hasło."); 
            }

            return Ok(response);
        }
    }
}
