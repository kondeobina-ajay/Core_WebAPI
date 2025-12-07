using Core_WebAPI.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using static Core_WebAPI.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Core_WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public AuthController(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        // ------------------ LOGIN ------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.PasswordHash))
                return BadRequest("Email and password are required");

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.email == request.email);

            if (manager == null)
            {
                return Ok(new { Success = false, Message = "User does not exist, please sign up first" });
            }

            if (!VerifyPassword(request.PasswordHash, manager.PasswordHash))
            {
                return Ok(new { Success = false, Message = "Email or password does not match" });
            }

            if (manager.JwtTokenExpiry == null || manager.JwtTokenExpiry <= DateTime.UtcNow)
            {
                manager.JwtToken = GenerateJwtToken(manager.email);
                manager.JwtTokenExpiry = DateTime.UtcNow.AddHours(2);
                await _context.SaveChangesAsync();
            }

            return Ok(new
            {
                Success = true,
                Message = "Login successful",
                Token = manager.JwtToken,
                TokenExpiry = manager.JwtTokenExpiry,
                Manager = manager
            });
        }




        // ------------------ SIGNUP ------------------
        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] Manager request)
        {
            if (string.IsNullOrEmpty(request.email) || string.IsNullOrEmpty(request.PasswordHash))
                return BadRequest("Email and password are required");

            var existing = await _context.Managers
                .FirstOrDefaultAsync(m => m.email == request.email);

            if (existing != null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Email already exists, please login"
                });
            }

            // Create new manager
            var manager = new Manager
            {
                email = request.email,
                Username = request.Username,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = HashPassword(request.PasswordHash)
            };

            // Generate JWT
            var token = GenerateJwtToken(manager.email);
            var expiry = DateTime.UtcNow.AddHours(2);

            manager.JwtToken = token;
            manager.JwtTokenExpiry = expiry;

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                IsNewUser = true,
                Message = "Signup successful",
                Token = token,
                TokenExpiry = expiry,
                Manager = manager
            });
        }



        // ------------------ HELPER METHODS ------------------
        private string GenerateJwtToken(string username)
        {
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"] ?? "YourSuperSecretKey123!");
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }



        [HttpGet("get-manager")]
        public async Task<IActionResult> GetManager(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest(new { Success = false, Message = "Email is required" });

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.email.ToLower() == email.ToLower());

            if (manager == null)
                return NotFound(new { Success = false, Message = "Manager not found" });

            return Ok(new
            {
                Success = true,
                Manager = new
                {
                    manager.Username,
                    manager.email,
                    manager.PhoneNumber
                }
            });
        }



        [HttpPut("update-manager")]
        public async Task<IActionResult> UpdateManager([FromBody] SaveSettingsDto request)
        {
            if (string.IsNullOrEmpty(request.OldEmail))
                return BadRequest(new { Success = false, Message = "Old email is required" });

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.email.ToLower() == request.OldEmail.ToLower());

            if (manager == null)
                return NotFound(new { Success = false, Message = "Manager not found" });

            // Update fields if provided
            manager.Username = string.IsNullOrEmpty(request.Username) ? manager.Username : request.Username;
            manager.PhoneNumber = string.IsNullOrEmpty(request.PhoneNumber) ? manager.PhoneNumber : request.PhoneNumber;
            manager.email = string.IsNullOrEmpty(request.Email) ? manager.email : request.Email;

            _context.Managers.Update(manager);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = "Settings updated successfully",
                Manager = new
                {
                    manager.Username,
                    manager.email,
                    manager.PhoneNumber
                }
            });
        }

        /// <summary>
        /// login request
        /// </summary>
        public class LoginRequest
        {
            public string email { get; set; } = null!;
            public string PasswordHash { get; set; } = null!;
        }



        public class SaveSettingsDto
        {
            [Required]
            public string OldEmail { get; set; } = null!; // used to find the manager in DB

            [Required]
            public string Email { get; set; } = null!; // new email
            public string Username { get; set; } = null!;

            public string? PhoneNumber { get; set; }
        }

    }
}





















//[HttpPost("login")]
//public async Task<IActionResult> Login([FromBody] Manager request)
//{
//    if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.PasswordHash))
//        return BadRequest("Username and password are required");

//    Manager manager = await _context.Managers
//        .FirstOrDefaultAsync(e => e.Username == request.Username);

//    if (manager == null)
//    {
//        // 1️⃣ User does not exist → register new
//        manager = new Manager
//        {
//            Username = request.Username,
//            PasswordHash = HashPassword(request.PasswordHash),
//        };

//        // Generate JWT for new user
//        var token = GenerateJwtToken(manager.Username);
//        var expiry = DateTime.UtcNow.AddHours(2);
//        manager.JwtToken = token;
//        manager.JwtTokenExpiry = expiry;

//        _context.Managers.Add(manager);
//        await _context.SaveChangesAsync();

//        return Ok(new
//        {
//            Success = true,
//            IsNewUser = true,
//            Message = "User does not exist, creating new user",
//            Token = token,
//            TokenExpiry = expiry,
//            Manager = manager
//        });
//    }
//    else
//    {
//        // 2️⃣ User exists → check password
//        if (!VerifyPassword(request.PasswordHash, manager.PasswordHash))
//        {
//            return Ok(new
//            {
//                Success = false,
//                Message = "Username or password does not match"
//            });
//        }

//        // 3️⃣ Check JWT expiration
//        if (manager.JwtTokenExpiry == null || manager.JwtTokenExpiry <= DateTime.UtcNow)
//        {
//            var newToken = GenerateJwtToken(manager.Username);
//            var newExpiry = DateTime.UtcNow.AddHours(2);
//            manager.JwtToken = newToken;
//            manager.JwtTokenExpiry = newExpiry;
//            await _context.SaveChangesAsync();
//        }

//        return Ok(new
//        {
//            Token = manager.JwtToken,
//            TokenExpiry = manager.JwtTokenExpiry,
//            Manager = manager
//        });
//    }
//}