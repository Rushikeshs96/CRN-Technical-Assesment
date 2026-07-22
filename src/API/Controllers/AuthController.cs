using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Replace this with actual database user validation
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _authService.GenerateToken(request.Username);
                return Ok(new AuthResponse(token));
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
