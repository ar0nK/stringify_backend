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
        private const string OrderStatus = "ORDERED";

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
                Datum = DateTime.UtcNow,
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

        public class CheckoutRequest
        {
            public string? Name { get; set; }
            public string? Address { get; set; }
            public string? MobilePhone { get; set; }
            public string? Email { get; set; }
        }

        private async Task<int> CalculateTotalAsync(List<RendelesTetel> items)
        {
            var total = 0;

            var termekIds = items
                .Where(t => t.TermekId != null && t.Darabszam > 0)
                .Select(t => t.TermekId!.Value)
                .Distinct()
                .ToList();

            var termekek = await _db.Termekek
                .AsNoTracking()
                .Where(t => termekIds.Contains(t.Id))
                .ToListAsync();

            var termekPriceById = termekek.ToDictionary(t => t.Id, t => t.Ar);

            total += items
                .Where(t => t.TermekId != null && t.Darabszam > 0)
                .Sum(t => (termekPriceById.TryGetValue(t.TermekId!.Value, out var price) ? price : 0) * t.Darabszam);

            var customIds = items
                .Where(t => t.EgyediGitarId != null && t.Darabszam > 0)
                .Select(t => t.EgyediGitarId!.Value)
                .Distinct()
                .ToList();

            if (customIds.Count == 0) return total;

            var customGuitars = await _db.EgyediGitarok
                .AsNoTracking()
                .Where(g => customIds.Contains(g.Id))
                .ToListAsync();

            var testformaIds = customGuitars.Select(g => g.TestformaId).Distinct().ToList();
            var neckIds = customGuitars.Select(g => g.NeckId).Distinct().ToList();
            var finishIds = customGuitars.Where(g => g.FinishId != null).Select(g => g.FinishId!.Value).Distinct().ToList();
            var pickguardIds = customGuitars.Where(g => g.PickguardId != null).Select(g => g.PickguardId!.Value).Distinct().ToList();

            var testformak = await _db.GitarTestformak.AsNoTracking().Where(t => testformaIds.Contains(t.Id)).ToListAsync();
            var nyakak = await _db.GitarNyakak.AsNoTracking().Where(n => neckIds.Contains(n.Id)).ToListAsync();
            var finishek = await _db.GitarFinishek.AsNoTracking().Where(f => finishIds.Contains(f.Id)).ToListAsync();
            var pickguardok = await _db.GitarPickguardok.AsNoTracking().Where(p => pickguardIds.Contains(p.Id)).ToListAsync();

            var testformaById = testformak.ToDictionary(t => t.Id);
            var nyakById = nyakak.ToDictionary(n => n.Id);
            var finishById = finishek.ToDictionary(f => f.Id);
            var pickguardById = pickguardok.ToDictionary(p => p.Id);

            var customPriceById = customGuitars.ToDictionary(g => g.Id, g =>
            {
                var price = 0;
                if (testformaById.TryGetValue(g.TestformaId, out var t)) price += t.Ar ?? 0;
                if (nyakById.TryGetValue(g.NeckId, out var n)) price += n.Ar;
                if (g.FinishId != null && finishById.TryGetValue(g.FinishId.Value, out var f)) price += f.Ar;
                if (g.PickguardId != null && pickguardById.TryGetValue(g.PickguardId.Value, out var p)) price += p.Ar;
                return price;
            });

            total += items
                .Where(t => t.EgyediGitarId != null && t.Darabszam > 0)
                .Sum(t => (customPriceById.TryGetValue(t.EgyediGitarId!.Value, out var price) ? price : 0) * t.Darabszam);

            return total;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");

            var cart = await GetOrCreateCartAsync(user.Id);

            var termekIds = cart.Tetelek
                .Where(t => t.TermekId != null && t.Darabszam > 0)
                .Select(t => t.TermekId!.Value)
                .Distinct()
                .ToList();
            var termekek = await _db.Termekek
                .AsNoTracking()
                .Include(t => t.TermekKepek)
                .Where(t => termekIds.Contains(t.Id))
                .ToListAsync();

            var productGroups = cart.Tetelek
                .Where(t => t.TermekId != null && t.Darabszam > 0)
                .GroupBy(t => t.TermekId!.Value)
                .Select(g =>
                {
                    var p = termekek.FirstOrDefault(x => x.Id == g.Key);
                    return new
                    {
                        type = "product",
                        productId = (int?)g.Key,
                        customGuitarId = (int?)null,
                        title = p?.Nev ?? "Ismeretlen termék",
                        price = p?.Ar ?? 0,
                        isAvailable = p?.Elerheto ?? false,
                        image = p?.TermekKepek != null ? (p.TermekKepek.Kep1 ?? "") : "",
                        quantity = g.Sum(x => x.Darabszam)
                    };
                })
                .ToList();

            var customIds = cart.Tetelek
                .Where(t => t.EgyediGitarId != null && t.Darabszam > 0)
                .Select(t => t.EgyediGitarId!.Value)
                .Distinct()
                .ToList();

            var customGuitars = await _db.EgyediGitarok
                .AsNoTracking()
                .Where(g => customIds.Contains(g.Id))
                .ToListAsync();

            var testformaIds = customGuitars.Select(g => g.TestformaId).Distinct().ToList();
            var neckIds = customGuitars.Select(g => g.NeckId).Distinct().ToList();
            var finishIds = customGuitars.Where(g => g.FinishId != null).Select(g => g.FinishId!.Value).Distinct().ToList();
            var pickguardIds = customGuitars.Where(g => g.PickguardId != null).Select(g => g.PickguardId!.Value).Distinct().ToList();

            var testformak = await _db.GitarTestformak.AsNoTracking().Where(t => testformaIds.Contains(t.Id)).ToListAsync();
            var nyakak = await _db.GitarNyakak.AsNoTracking().Where(n => neckIds.Contains(n.Id)).ToListAsync();
            var finishek = await _db.GitarFinishek.AsNoTracking().Where(f => finishIds.Contains(f.Id)).ToListAsync();
            var pickguardok = await _db.GitarPickguardok.AsNoTracking().Where(p => pickguardIds.Contains(p.Id)).ToListAsync();

            var testformaById = testformak.ToDictionary(t => t.Id);
            var nyakById = nyakak.ToDictionary(n => n.Id);
            var finishById = finishek.ToDictionary(f => f.Id);
            var pickguardById = pickguardok.ToDictionary(p => p.Id);

            var customGroups = cart.Tetelek
                .Where(t => t.EgyediGitarId != null && t.Darabszam > 0)
                .GroupBy(t => t.EgyediGitarId!.Value)
                .Select(g =>
                {
                    var custom = customGuitars.FirstOrDefault(x => x.Id == g.Key);
                    GitarTestforma? testforma = null;
                    GitarNyak? nyak = null;
                    GitarFinish? finish = null;
                    GitarPickguard? pickguard = null;

                    if (custom != null)
                    {
                        testformaById.TryGetValue(custom.TestformaId, out testforma);
                        nyakById.TryGetValue(custom.NeckId, out nyak);
                        if (custom.FinishId != null) finishById.TryGetValue(custom.FinishId.Value, out finish);
                        if (custom.PickguardId != null) pickguardById.TryGetValue(custom.PickguardId.Value, out pickguard);
                    }

                    var titleParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(testforma?.Nev)) titleParts.Add(testforma.Nev);
                    if (!string.IsNullOrWhiteSpace(finish?.Nev)) titleParts.Add(finish.Nev);
                    if (!string.IsNullOrWhiteSpace(pickguard?.Nev)) titleParts.Add(pickguard.Nev);
                    if (!string.IsNullOrWhiteSpace(nyak?.Nev)) titleParts.Add(nyak.Nev);
                    var title = titleParts.Count > 0 ? string.Join(" - ", titleParts) : "Egyedi gitár";

                    var price = (testforma?.Ar ?? 0) + (nyak?.Ar ?? 0) + (finish?.Ar ?? 0) + (pickguard?.Ar ?? 0);
                    var image = finish?.KepUrl ?? pickguard?.KepUrl ?? nyak?.KepUrl ?? "";

                    return new
                    {
                        type = "custom",
                        productId = (int?)null,
                        customGuitarId = (int?)g.Key,
                        title,
                        price,
                        isAvailable = true,
                        image,
                        quantity = g.Sum(x => x.Darabszam)
                    };
                })
                .ToList();

            var allItems = productGroups.Concat(customGroups).ToList();
            var total = allItems.Sum(i => i.price * i.quantity);
            cart.Osszeg = total;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                orderId = cart.Id,
                status = cart.Status,
                total,
                items = allItems
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

                var item = await _db.RendelesTetelek
                    .FirstOrDefaultAsync(t => t.RendelesId == cart.Id && t.TermekId == req.TermekId.Value);

                if (item == null)
                {
                    _db.RendelesTetelek.Add(new RendelesTetel
                    {
                        RendelesId = cart.Id,
                        TermekId = req.TermekId.Value,
                        Darabszam = qty
                    });
                }
                else
                {
                    item.Darabszam += qty;
                }
            }
            else
            {
                var exists = await _db.EgyediGitarok.AsNoTracking().AnyAsync(g => g.Id == req.EgyediGitarId!.Value && g.FelhasznaloId == user.Id);
                if (!exists) return NotFound("Egyedi gitár nem található");

                var item = await _db.RendelesTetelek
                    .FirstOrDefaultAsync(t => t.RendelesId == cart.Id && t.EgyediGitarId == req.EgyediGitarId!.Value);

                if (item == null)
                {
                    _db.RendelesTetelek.Add(new RendelesTetel
                    {
                        RendelesId = cart.Id,
                        EgyediGitarId = req.EgyediGitarId!.Value,
                        Darabszam = qty
                    });
                }
                else
                {
                    item.Darabszam += qty;
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

                if (targetQty == 0)
                {
                    _db.RendelesTetelek.RemoveRange(rows);
                }
                else if (rows.Count == 0)
                {
                    _db.RendelesTetelek.Add(new RendelesTetel
                    {
                        RendelesId = cart.Id,
                        TermekId = req.TermekId.Value,
                        Darabszam = targetQty
                    });
                }
                else
                {
                    rows[0].Darabszam = targetQty;
                    if (rows.Count > 1) _db.RendelesTetelek.RemoveRange(rows.Skip(1));
                }
            }
            else
            {
                var rows = await _db.RendelesTetelek
                    .Where(t => t.RendelesId == cart.Id && t.EgyediGitarId == req.EgyediGitarId!.Value)
                    .ToListAsync();

                if (targetQty == 0)
                {
                    _db.RendelesTetelek.RemoveRange(rows);
                }
                else if (rows.Count == 0)
                {
                    _db.RendelesTetelek.Add(new RendelesTetel
                    {
                        RendelesId = cart.Id,
                        EgyediGitarId = req.EgyediGitarId!.Value,
                        Darabszam = targetQty
                    });
                }
                else
                {
                    rows[0].Darabszam = targetQty;
                    if (rows.Count > 1) _db.RendelesTetelek.RemoveRange(rows.Skip(1));
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

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest? req)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");

            var cart = await GetOrCreateCartAsync(user.Id);
            var cartItems = await _db.RendelesTetelek
                .Where(t => t.RendelesId == cart.Id && t.Darabszam > 0)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                return BadRequest("A kosár üres");
            }

            var total = await CalculateTotalAsync(cartItems);

            var order = new Rendeles
            {
                FelhasznaloId = user.Id,
                Status = OrderStatus,
                Osszeg = total,
                Datum = DateTime.UtcNow
            };

            _db.Rendelesek.Add(order);
            await _db.SaveChangesAsync();

            var orderItems = cartItems.Select(t => new RendelesTetel
            {
                RendelesId = order.Id,
                TermekId = t.TermekId,
                EgyediGitarId = t.EgyediGitarId,
                Darabszam = t.Darabszam
            }).ToList();

            _db.RendelesTetelek.AddRange(orderItems);
            _db.RendelesTetelek.RemoveRange(cartItems);
            cart.Osszeg = 0;

            await _db.SaveChangesAsync();

            return Ok(new { orderId = order.Id, total = order.Osszeg });
        }
    }
}
