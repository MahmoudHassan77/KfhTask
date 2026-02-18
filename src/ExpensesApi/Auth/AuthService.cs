
using ExpensesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ExpensesApi.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IOptions<JwtSettngs> _jwtSettings;
        private readonly AppDbContext _context;

        public AuthService(ILogger<AuthService> logger, IOptions<JwtSettngs> jwtSettings, AppDbContext context)
        {
            _logger = logger;
            _jwtSettings = jwtSettings;
            _context = context;
        }
        private string GenerateToken(string userId, string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, userId),
                new System.Security.Claims.Claim(JwtRegisteredClaimNames.UniqueName, username)
            };
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpiryInMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token); 
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequist request)
        {
            var (isValid, userId) = await ValidateCredentialAsync(request.Username, request.Password);
            if(!isValid && userId is null)
            {
                return new LoginResponse(null, null, null, "Invalid username or password");
            }

            var token = GenerateToken(userId!, request.Username);
            return new LoginResponse(token, request.Username, userId, "Login successful");

        }

        public async Task<RegisterResponce?> RegisterAsync(RegisterRequest request)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserName == request.Username);
            if(existingUser != null)
            {
                return new RegisterResponce(null, null, "User already exist");
            }
            var userId = $"usr-{Guid.NewGuid().ToString().Substring(0, 20)}";
            var user = new Models.User
            {
                Id = userId,
                UserName = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                CreatedAt = DateTime.UtcNow
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new RegisterResponce(userId, request.Username, "User registered succefully");
        }

        private async Task<(bool Isvalid, string? UserId)> ValidateCredentialAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if(user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return (true, user.Id);
            }
            return (false, null);
        }
    }
}
