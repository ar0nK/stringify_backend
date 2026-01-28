using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Models;

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

        [HttpPost]
        public async Task<ActionResult<EgyediGitar>> Create(EgyediGitar gitar)
        {
            _context.EgyediGitarok.Add(gitar);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = gitar.Id }, gitar);
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
}
