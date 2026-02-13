using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.DTOs;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly StringifyDbContext _context;

        public LoginController(StringifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetSalt/{email}")]
        public async Task<IActionResult> GetSalt(string email)
        {
            try
            {
                User response = await _context.Users
                    .FirstOrDefaultAsync(f => f.Email == email);

                return response == null
                    ? BadRequest("Felhasználó nem található")
                    : Ok(response.Salt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                string Hash = Program.CreateSHA256(loginDTO.TmpHash);

                User loggedUser = await _context.Users
                    .FirstOrDefaultAsync(f => f.Email == loginDTO.Email && f.Jelszo == Hash);

                if (loggedUser != null && loggedUser.Aktiv == 1)
                {
                    string token = Guid.NewGuid().ToString();

                    lock (Program.LoggedInUsers)
                    {
                        Program.LoggedInUsers[token] = loggedUser;
                    }

                    return Ok(new LoggedUser
                    {
                        Name = loggedUser.Nev,
                        Email = loggedUser.Email,
                        Permission = loggedUser.Jogosultsag,
                        Token = token
                    });
                }
                else
                {
                    return BadRequest("Hibás email vagy jelszó/inaktív felhasználó!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Hiba történt: {ex.Message}");
            }
        }
    }
}