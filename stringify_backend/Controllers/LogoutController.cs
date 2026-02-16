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
            return Ok("Sikeres kijelentkezés. A kliens oldalon távolítsd el a tokent.");
        }
    }

    public class LogoutDTO
    {
        public string Token { get; set; } = string.Empty;
    }
}