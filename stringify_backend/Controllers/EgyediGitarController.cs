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
        [Authorize]
        public async Task<ActionResult<IEnumerable<EgyediGitar>>> GetAll()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            return await _context.EgyediGitarok
                .Where(g => g.FelhasznaloId == userId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EgyediGitar>> Get(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var gitar = await _context.EgyediGitarok.FirstOrDefaultAsync(g => g.Id == id && g.FelhasznaloId == userId);
            if (gitar == null) return NotFound();
            return gitar;
        }

        [HttpPost("save")]
        [Authorize]
        public async Task<IActionResult> Save([FromBody] EgyediGitarSaveDto dto)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var hasValidTestforma = await _context.GitarTestformak.AnyAsync(t => t.Id == dto.TestformaId);
            var hasValidNyak = await _context.GitarNyakak.AnyAsync(n => n.Id == dto.NeckId);
            var hasValidFinish = dto.FinishId == null || await _context.GitarFinishek.AnyAsync(f => f.Id == dto.FinishId.Value && f.TestFormaId == dto.TestformaId);
            var hasValidPickguard = dto.PickguardId == null || await _context.GitarPickguardok.AnyAsync(p => p.Id == dto.PickguardId.Value && p.TestFormaId == dto.TestformaId);

            if (!hasValidTestforma || !hasValidNyak || !hasValidFinish || !hasValidPickguard)
            {
                return BadRequest();
            }

            var gitar = new EgyediGitar
            {
                FelhasznaloId = userId,
                TestformaId   = dto.TestformaId,
                FinishId      = dto.FinishId,
                PickguardId   = dto.PickguardId,
                NeckId        = dto.NeckId,
                Letrehozva    = DateTime.UtcNow
            };

            _context.EgyediGitarok.Add(gitar);
            await _context.SaveChangesAsync();

            return Ok(new { id = gitar.Id });
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, EgyediGitar gitar)
        {
            if (id != gitar.Id) return BadRequest();

            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var existing = await _context.EgyediGitarok.FirstOrDefaultAsync(g => g.Id == id && g.FelhasznaloId == userId);
            if (existing == null) return NotFound();

            var hasValidTestforma = await _context.GitarTestformak.AnyAsync(t => t.Id == gitar.TestformaId);
            var hasValidNyak = await _context.GitarNyakak.AnyAsync(n => n.Id == gitar.NeckId);
            var hasValidFinish = gitar.FinishId == null || await _context.GitarFinishek.AnyAsync(f => f.Id == gitar.FinishId.Value && f.TestFormaId == gitar.TestformaId);
            var hasValidPickguard = gitar.PickguardId == null || await _context.GitarPickguardok.AnyAsync(p => p.Id == gitar.PickguardId.Value && p.TestFormaId == gitar.TestformaId);

            if (!hasValidTestforma || !hasValidNyak || !hasValidFinish || !hasValidPickguard)
            {
                return BadRequest();
            }

            existing.TestformaId = gitar.TestformaId;
            existing.FinishId = gitar.FinishId;
            existing.PickguardId = gitar.PickguardId;
            existing.NeckId = gitar.NeckId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();

            var gitar = await _context.EgyediGitarok.FirstOrDefaultAsync(g => g.Id == id && g.FelhasznaloId == userId);
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
