using Microsoft.AspNetCore.Mvc;

namespace stringify_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        [HttpPost]
        public IActionResult Logout([FromBody] LogoutDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                return BadRequest("Token hiányzik.");
            }

            lock (Program.LoggedInUsers)
            {
                if (Program.LoggedInUsers.ContainsKey(dto.Token))
                {
                    Program.LoggedInUsers.Remove(dto.Token);
                }
            }

            return Ok("Sikeres kijelentkezés.");
        }
    }

    public class LogoutDTO
    {
        public string Token { get; set; } = string.Empty;
    }
}