using ExpensesApi.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpensesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);
            if (result?.UserId == null)
            {
                _logger.LogWarning("Registration failed for user {Username}: {Message}", request.Username, result?.Message);
                return BadRequest(result?.Message);
            }
            _logger.LogInformation("User {Username} registered successfully with ID {UserId}", request.Username, result.UserId);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequist request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _authService.LoginAsync(request);
            if (result?.Token == null)
            {
                _logger.LogWarning("Login failed for user {Username}", request.Username);
                return Unauthorized();
            }
            _logger.LogInformation("User {Username} logged in successfully", request.Username);
            return Ok(result);
        }
    }
}
