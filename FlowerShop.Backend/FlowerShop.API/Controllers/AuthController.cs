using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.API.Data;
using FlowerShop.API.Models;
using FlowerShop.API.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace FlowerShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly FlowerShopContext _context;

        public AuthController(FlowerShopContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            try
            {
                Console.WriteLine($"Login attempt for username: {request.Username}");

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

                Console.WriteLine($"User found: {user != null}");

                if (user != null)
                {
                    var inputHash = HashPassword(request.Password);
                    Console.WriteLine($"Input hash: {inputHash}");
                    Console.WriteLine($"Stored hash: {user.PasswordHash}");
                    Console.WriteLine($"Hash match: {inputHash == user.PasswordHash}");
                }

                if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
                {
                    return BadRequest(new { message = "Kullanıcı adı veya şifre hatalı." });
                }

                // Update last login
                user.LastLoginAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var response = new LoginResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Token = GenerateSimpleToken(user.Id, user.Username, user.Role)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Giriş işlemi sırasında bir hata oluştu." });
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register(RegisterRequest request)
        {
            try
            {
                // Check if username or email already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

                if (existingUser != null)
                {
                    return BadRequest(new { message = "Bu kullanıcı adı veya e-posta zaten kullanılıyor." });
                }

                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PasswordHash = HashPassword(request.Password),
                    Role = "Customer",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var response = new LoginResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Token = GenerateSimpleToken(user.Id, user.Username, user.Role)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Kayıt işlemi sırasında bir hata oluştu." });
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "FlowerShopSalt"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        private string GenerateSimpleToken(int userId, string username, string role)
        {
            // Simple token for demo purposes - in production use JWT
            var tokenData = $"{userId}:{username}:{role}:{DateTime.UtcNow.Ticks}";
            var tokenBytes = Encoding.UTF8.GetBytes(tokenData);
            return Convert.ToBase64String(tokenBytes);
        }

        // Test endpoint to generate hash
        [HttpGet("test-hash/{password}")]
        public ActionResult<string> TestHash(string password)
        {
            return HashPassword(password);
        }

        // Test endpoint to check users
        [HttpGet("test-users")]
        public async Task<ActionResult<object>> TestUsers()
        {
            var users = await _context.Users.Select(u => new { u.Id, u.Username, u.Email, u.PasswordHash, u.IsActive }).ToListAsync();
            return Ok(users);
        }
    }
}
