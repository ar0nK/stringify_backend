using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly StringifyDbContext _db;
        private const string CartStatus = "CART";

        public CartController(StringifyDbContext db)
        {
            _db = db;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId)) return null;
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Aktiv == 1);
        }

        private async Task<Rendeles> GetOrCreateCartAsync(int userId)
        {
            var cart = await _db.Rendelesek
                .Include(r => r.Tetelek)
                .FirstOrDefaultAsync(r => r.FelhasznaloId == userId && r.Status == CartStatus);

            if (cart != null) return cart;

            cart = new Rendeles
            {
                FelhasznaloId = userId,
                Status = CartStatus,
                Osszeg = 0,
                Datum = DateTime.Now,
            };

            _db.Rendelesek.Add(cart);
            await _db.SaveChangesAsync();

            cart = await _db.Rendelesek
                .Include(r => r.Tetelek)
                .FirstAsync(r => r.Id == cart.Id);

            return cart;
        }

        public class CartItemRequest
        {
            public int? TermekId { get; set; }
            public int? EgyediGitarId { get; set; }
            public int Quantity { get; set; } = 1;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");

            var cart = await GetOrCreateCartAsync(user.Id);

            var termekIds = cart.Tetelek.Where(t => t.TermekId != null).Select(t => t.TermekId!.Value).Distinct().ToList();
            var termekek = await _db.Termekek
                .AsNoTracking()
                .Include(t => t.TermekKepek)
                .Where(t => termekIds.Contains(t.Id))
                .ToListAsync();

            var productGroups = cart.Tetelek
                .Where(t => t.TermekId != null)
                .GroupBy(t => t.TermekId!.Value)
                .Select(g =>
                {
                    var p = termekek.FirstOrDefault(x => x.Id == g.Key);
                    return new
                    {
                        type = "product",
                        productId = g.Key,
                        title = p?.Nev ?? "Ismeretlen termék",
                        price = p?.Ar ?? 0,
                        isAvailable = p?.Elerheto ?? false,
                        image = p?.TermekKepek != null ? (p.TermekKepek.Kep1 ?? "") : "",
                        quantity = g.Count()
                    };
                })
                .ToList();

            var total = productGroups.Sum(i => i.price * i.quantity);
            cart.Osszeg = total;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                orderId = cart.Id,
                status = cart.Status,
                total,
                items = productGroups
            });
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItemRequest req)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");

            if (req.TermekId == null && req.EgyediGitarId == null)
                return BadRequest("TermekId vagy EgyediGitarId kötelező");

            var qty = Math.Max(1, req.Quantity);
            var cart = await GetOrCreateCartAsync(user.Id);

            if (req.TermekId != null)
            {
                var exists = await _db.Termekek.AsNoTracking().AnyAsync(t => t.Id == req.TermekId.Value);
                if (!exists) return NotFound("Termék nem található");

                for (int i = 0; i < qty; i++)
                {
                    _db.RendelesTetelek.Add(new RendelesTetel
                    {
                        RendelesId = cart.Id,
                        TermekId = req.TermekId.Value
                    });
                }
            }
            else
            {
                var exists = await _db.EgyediGitarok.AsNoTracking().AnyAsync(g => g.Id == req.EgyediGitarId!.Value);
                if (!exists) return NotFound("Egyedi gitár nem található");

                for (int i = 0; i < qty; i++)
                {
                    _db.RendelesTetelek.Add(new RendelesTetel
                    {
                        RendelesId = cart.Id,
                        EgyediGitarId = req.EgyediGitarId!.Value
                    });
                }
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Kosár frissítve" });
        }

        [HttpPut("set")]
        public async Task<IActionResult> SetQuantity([FromBody] CartItemRequest req)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");
            if (req.TermekId == null && req.EgyediGitarId == null)
                return BadRequest("TermekId vagy EgyediGitarId kötelező");

            var cart = await GetOrCreateCartAsync(user.Id);
            var targetQty = Math.Max(0, req.Quantity);

            if (req.TermekId != null)
            {
                var rows = await _db.RendelesTetelek
                    .Where(t => t.RendelesId == cart.Id && t.TermekId == req.TermekId.Value)
                    .ToListAsync();

                var currentQty = rows.Count;
                if (targetQty == currentQty) return Ok(new { message = "Nincs változás" });

                if (targetQty == 0)
                {
                    _db.RendelesTetelek.RemoveRange(rows);
                }
                else if (targetQty > currentQty)
                {
                    for (int i = 0; i < (targetQty - currentQty); i++)
                        _db.RendelesTetelek.Add(new RendelesTetel { RendelesId = cart.Id, TermekId = req.TermekId.Value });
                }
                else
                {
                    _db.RendelesTetelek.RemoveRange(rows.Take(currentQty - targetQty));
                }
            }
            else
            {
                var rows = await _db.RendelesTetelek
                    .Where(t => t.RendelesId == cart.Id && t.EgyediGitarId == req.EgyediGitarId!.Value)
                    .ToListAsync();
                var currentQty = rows.Count;

                if (targetQty == 0)
                {
                    _db.RendelesTetelek.RemoveRange(rows);
                }
                else if (targetQty > currentQty)
                {
                    for (int i = 0; i < (targetQty - currentQty); i++)
                        _db.RendelesTetelek.Add(new RendelesTetel { RendelesId = cart.Id, EgyediGitarId = req.EgyediGitarId!.Value });
                }
                else if (targetQty < currentQty)
                {
                    _db.RendelesTetelek.RemoveRange(rows.Take(currentQty - targetQty));
                }
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Mennyiség frissítve" });
        }

        [HttpDelete("remove/product/{termekId:int}")]
        public async Task<IActionResult> RemoveProduct(int termekId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");
            var cart = await GetOrCreateCartAsync(user.Id);

            var rows = await _db.RendelesTetelek
                .Where(t => t.RendelesId == cart.Id && t.TermekId == termekId)
                .ToListAsync();
            _db.RendelesTetelek.RemoveRange(rows);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Tétel törölve" });
        }

        [HttpPost("clear")]
        public async Task<IActionResult> Clear()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");
            var cart = await GetOrCreateCartAsync(user.Id);

            var rows = await _db.RendelesTetelek.Where(t => t.RendelesId == cart.Id).ToListAsync();
            _db.RendelesTetelek.RemoveRange(rows);
            cart.Osszeg = 0;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Kosár kiürítve" });
        }
    }
}
