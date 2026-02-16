using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using stringify_backend.DTOs;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly StringifyDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(StringifyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                    string token = CreateJwt(loggedUser);

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

        private string CreateJwt(User user)
        {
            var key = _configuration["Jwt:Key"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("JWT Key is missing from configuration.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("permission", user.Jogosultsag.ToString())
            };

            var expiresInMinutes = int.TryParse(_configuration["Jwt:ExpirationMinutes"], out var minutes)
                ? minutes
                : 60;

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}