using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.DTOs;
using stringify_backend.Models;
using System.Security.Cryptography;

namespace stringify_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly StringifyDbContext _context;

        public RegisterController(StringifyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                var normalizedEmail = registerDTO.Email.Trim().ToLowerInvariant();
                var normalizedName = registerDTO.Nev.Trim();

                if (string.IsNullOrWhiteSpace(normalizedName) ||
                    string.IsNullOrWhiteSpace(normalizedEmail) ||
                    string.IsNullOrWhiteSpace(registerDTO.Jelszo))
                {
                    return BadRequest("Minden mező kitöltése kötelező.");
                }

                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == normalizedEmail);

                if (existingUser != null)
                {
                    return BadRequest("Ez az email cím már használatban van!");
                }

                string salt = GenerateRandomSalt();

                string tmpHash = Program.CreateSHA256(registerDTO.Jelszo + salt);

                string finalHash = Program.CreateSHA256(tmpHash);

                var newUser = new User
                {
                    Nev = normalizedName,
                    Email = normalizedEmail,
                    Jelszo = finalHash,
                    Salt = salt,
                    Jogosultsag = 1,
                    Aktiv = 1 
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok("Sikeres regisztráció! Most már bejelentkezhetsz.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba történt a regisztráció során: {ex.Message}");
            }
        }

        private string GenerateRandomSalt()
        {
            byte[] saltBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }
    }
}
