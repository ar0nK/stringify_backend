using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.DTOs;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly StringifyDbContext _context;

        public RegisterController(StringifyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(dto.Nev) ||
                    string.IsNullOrWhiteSpace(dto.Email) ||
                    string.IsNullOrWhiteSpace(dto.Jelszo))
                {
                    return BadRequest("Minden mező kitöltése kötelező.");
                }

                // Check if email already exists
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                {
                    return BadRequest("Ez az email már regisztrált.");
                }

                // Generate salt and hash password
                string salt = Program.GenerateSalt();
                string hashedPassword = Program.CreateSHA256(dto.Jelszo + salt);

                var user = new User
                {
                    Nev = dto.Nev,
                    Email = dto.Email,
                    Telefonszam = dto.Telefonszam ?? "",
                    Salt = salt,
                    Jelszo = hashedPassword,
                    Jogosultsag = 1,
                    Aktiv = 1
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("Sikeres regisztráció!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Hiba történt: {ex.Message}");
            }
        }
    }
}