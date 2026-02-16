using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KedvencTermekController : ControllerBase
    {
        private readonly StringifyDbContext _context;

        public KedvencTermekController(StringifyDbContext context)
        {
            _context = context;
        }

        // Helper method to get current user from token
        private User? GetCurrentUser()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            lock (Program.LoggedInUsers)
            {
                if (Program.LoggedInUsers.ContainsKey(token))
                {
                    return Program.LoggedInUsers[token];
                }
            }

            return null;
        }

        // GET: api/kedvencetermek - Get all favorites for logged-in user
        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetUserFavorites()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return Unauthorized("Kérjük, jelentkezz be!");
            }

            var favoriteProductIds = await _context.KedvencTermekek
                .Where(kt => kt.FelhasznaloId == currentUser.Id)
                .Select(kt => kt.TermekId)
                .ToListAsync();

            return Ok(favoriteProductIds);
        }

        // GET: api/kedvencetermek/products - Get full product details for favorites
        [HttpGet("products")]
        public async Task<ActionResult> GetUserFavoriteProducts()
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return Unauthorized("Kérjük, jelentkezz be!");
            }

            var favoriteProducts = await _context.KedvencTermekek
                .Where(kt => kt.FelhasznaloId == currentUser.Id)
                .Include(kt => kt.Termek)
                .ThenInclude(t => t.TermekKepek)
                .Select(kt => new
                {
                    id = kt.Termek.Id,
                    title = kt.Termek.Nev,
                    shortDescription = kt.Termek.RovidLeiras,
                    longDescription = kt.Termek.Leiras,
                    previewDescription = kt.Termek.RovidLeiras ?? "",
                    price = kt.Termek.Ar,
                    isAvailable = kt.Termek.Elerheto,
                    images = kt.Termek.TermekKepek != null
                        ? new List<string>
                        {
                            kt.Termek.TermekKepek.Kep1,
                            kt.Termek.TermekKepek.Kep2,
                            kt.Termek.TermekKepek.Kep3,
                            kt.Termek.TermekKepek.Kep4,
                            kt.Termek.TermekKepek.Kep5
                        }
                        .Where(url => !string.IsNullOrWhiteSpace(url))
                        .ToList()
                        : new List<string>(),
                    rating = (double?)null,
                    reviewCount = (int?)null,
                    savedAt = kt.Letrehozva
                })
                .ToListAsync();

            return Ok(favoriteProducts);
        }

        // POST: api/kedvencetermek/{termekId} - Add product to favorites
        [HttpPost("{termekId}")]
        public async Task<IActionResult> AddFavorite(int termekId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return Unauthorized("Kérjük, jelentkezz be!");
            }

            // Check if product exists
            var product = await _context.Termekek.FindAsync(termekId);
            if (product == null)
            {
                return NotFound("Termék nem található");
            }

            // Check if already favorited
            var existingFavorite = await _context.KedvencTermekek
                .FirstOrDefaultAsync(kt => kt.FelhasznaloId == currentUser.Id && kt.TermekId == termekId);

            if (existingFavorite != null)
            {
                return BadRequest("Ez a termék már a kedvencek között van");
            }

            // Add to favorites
            var kedvenc = new KedvencTermek
            {
                FelhasznaloId = currentUser.Id,
                TermekId = termekId,
                Letrehozva = DateTime.Now
            };

            _context.KedvencTermekek.Add(kedvenc);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Termék hozzáadva a kedvencekhez" });
        }

        // DELETE: api/kedvencetermek/{termekId} - Remove product from favorites
        [HttpDelete("{termekId}")]
        public async Task<IActionResult> RemoveFavorite(int termekId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return Unauthorized("Kérjük, jelentkezz be!");
            }

            var favorite = await _context.KedvencTermekek
                .FirstOrDefaultAsync(kt => kt.FelhasznaloId == currentUser.Id && kt.TermekId == termekId);

            if (favorite == null)
            {
                return NotFound("Kedvenc nem található");
            }

            _context.KedvencTermekek.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Termék eltávolítva a kedvencekből" });
        }

        [HttpPost("toggle/{termekId}")]
        public async Task<IActionResult> ToggleFavorite(int termekId)
        {
            var currentUser = GetCurrentUser();
            if (currentUser == null)
            {
                return Unauthorized("Kérjük, jelentkezz be!");
            }

            var product = await _context.Termekek.FindAsync(termekId);
            if (product == null)
            {
                return NotFound("Termék nem található");
            }

            var existingFavorite = await _context.KedvencTermekek
                .FirstOrDefaultAsync(kt => kt.FelhasznaloId == currentUser.Id && kt.TermekId == termekId);

            if (existingFavorite != null)
            {
                _context.KedvencTermekek.Remove(existingFavorite);
                await _context.SaveChangesAsync();
                return Ok(new { isFavorite = false, message = "Termék eltávolítva a kedvencekből" });
            }
            else
            {
                var kedvenc = new KedvencTermek
                {
                    FelhasznaloId = currentUser.Id,
                    TermekId = termekId,
                    Letrehozva = DateTime.Now
                };

                _context.KedvencTermekek.Add(kedvenc);
                await _context.SaveChangesAsync();
                return Ok(new { isFavorite = true, message = "Termék hozzáadva a kedvencekhez" });
            }
        }
    }
}