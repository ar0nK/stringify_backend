using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Register([FromBody] RegisterDTO dto)
        {
            // Check if email already exists
            if (_context.Users.Any(u => u.Email == dto.Email))
            {
                return BadRequest("Ez az email már regsiztrált.");
            }

            // Generate salt and hash password
            string salt = Program.GenerateSalt();
            string hashedPassword = Program.CreateSHA256(dto.Jelszo + salt);

            var user = new User
            {
                Nev = dto.Nev,
                Email = dto.Email,
                Telefonszam = dto.Telefonszam,
                Salt = salt,
                Jelszo = hashedPassword,
                Jogosultsag = 1, // Default permission level
                Aktiv = 1        // Set as active
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("Registration successful.");
        }
    }
}
