using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.DTOs;
using stringify_backend.Models;
using stringify_backend.DTOs;
using stringify_backend.Models;
using stringify_backend;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("GetSalt/{felhasznaloNev}")]
        public async Task<IActionResult> GetSalt(string felhasznaloNev)
        {
            
            using (var cx = new StringifyDbContext())
            {
                try
                {
                    User response = await cx.Users.FirstOrDefaultAsync(f => f.Nev == felhasznaloNev);
                    return response == null ? BadRequest("Hiba") : Ok(response.Salt);
                }
                catch
                (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost]

        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            using (var cx = new StringifyDbContext())
            {
                try
                {
                    string Hash = Program.CreateSHA256(loginDTO.TmpHash);
                    User loggedUser = await cx.Users.FirstOrDefaultAsync(f => f.Nev == loginDTO.LoginName && f.Jelszo == Hash);
                    if (loggedUser != null && loggedUser.Aktiv == 1)
                    {
                        string token = Guid.NewGuid().ToString();
                        lock (Program.LoggedInUsers)
                        {
                            Program.LoggedInUsers.Add(token, loggedUser);
                        }
                        return Ok(new LoggedUser { Name = loggedUser.Nev, Email = loggedUser.Email, Permission = loggedUser.Jogosultsag, Token = token });
                    }
                    else
                    {
                        return BadRequest("Hibás név vagy jelszó/inaktív felhasználó!");
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new LoggedUser { Permission = -1, Name = ex.Message, Email = "" });
                }
            }
        }
    }
}
