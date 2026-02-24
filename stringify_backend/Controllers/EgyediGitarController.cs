using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Models;
using System.Security.Claims;

namespace stringify_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EgyediGitarController : ControllerBase
    {
        private readonly StringifyDbContext _context;

        public EgyediGitarController(StringifyDbContext context)
        {
            _context = context;
        }

        [HttpGet("options")]
        public async Task<IActionResult> GetOptions()
        {
            var testformak = await _context.GitarTestformak
                .Select(t => new { t.Id, t.Nev, t.Leiras, t.Ar })
                .ToListAsync();

            var finishek = await _context.GitarFinishek
                .Select(f => new { f.Id, f.Nev, f.KepUrl, f.Ar, f.TestFormaId, f.ZIndex })
                .ToListAsync();

            var pickguardok = await _context.GitarPickguardok
                .Select(p => new { p.Id, p.Nev, p.KepUrl, p.Ar, p.TestFormaId, p.ZIndex })
                .ToListAsync();

            var nyakak = await _context.GitarNyakak
                .Select(n => new { n.Id, n.Nev, n.KepUrl, n.Ar, n.ZIndex })
                .ToListAsync();

            return Ok(new { testformak, finishek, pickguardok, nyakak });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EgyediGitar>>> GetAll()
        {
            return await _context.EgyediGitarok.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EgyediGitar>> Get(int id)
        {
            var gitar = await _context.EgyediGitarok.FindAsync(id);
            if (gitar == null) return NotFound();
            return gitar;
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] EgyediGitarSaveDto dto)
        {
            var userIdStr = User.FindFirstValue("Id");
            if (userIdStr == null) return Unauthorized();

            var gitar = new EgyediGitar
            {
                FelhasznaloId = int.Parse(userIdStr),
                TestformaId   = dto.TestformaId,
                FinishId      = dto.FinishId,
                PickguardId   = dto.PickguardId,
                NeckId        = dto.NeckId,
                Letrehozva    = DateTime.Now
            };

            _context.EgyediGitarok.Add(gitar);
            await _context.SaveChangesAsync();

            return Ok(new { id = gitar.Id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EgyediGitar gitar)
        {
            if (id != gitar.Id) return BadRequest();
            _context.Entry(gitar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gitar = await _context.EgyediGitarok.FindAsync(id);
            if (gitar == null) return NotFound();
            _context.EgyediGitarok.Remove(gitar);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    public class EgyediGitarSaveDto
    {
        public int  TestformaId { get; set; }
        public int? FinishId    { get; set; }
        public int? PickguardId { get; set; }
        public int  NeckId      { get; set; }
    }
}