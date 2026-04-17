using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using stringify_backend.Models;

namespace stringify_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RendelesController : ControllerBase
    {
        private readonly StringifyDbContext _db;
        private const string OrderStatus = "ORDERED";

        public RendelesController(StringifyDbContext db)
        {
            _db = db;
        }

        private async Task<User?> GetCurrentUserAsync()
        {
            var userIdValue =
                User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                User.FindFirstValue("sub") ??
                User.FindFirstValue("nameid");
            if (!int.TryParse(userIdValue, out var userId)) return null;
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == userId && u.Aktiv == 1);
        }

        private record CustomGuitarDto(int Id, string Name, int Price, string Image, string? Testforma, string? Neck, string? Finish, string? Pickguard, string? FinishImage, string? PickguardImage, string? NeckImage);

        private async Task<Dictionary<int, CustomGuitarDto>> LoadCustomGuitarDtosAsync(List<int> customIds)
        {
            if (customIds.Count == 0) return new Dictionary<int, CustomGuitarDto>();

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

            return customGuitars.ToDictionary(g => g.Id, g =>
            {
                testformaById.TryGetValue(g.TestformaId, out var testforma);
                nyakById.TryGetValue(g.NeckId, out var neck);
                finishById.TryGetValue(g.FinishId ?? 0, out var finish);
                pickguardById.TryGetValue(g.PickguardId ?? 0, out var pickguard);

                var titleParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(testforma?.Nev)) titleParts.Add(testforma.Nev);
                if (!string.IsNullOrWhiteSpace(finish?.Nev)) titleParts.Add(finish.Nev);
                if (!string.IsNullOrWhiteSpace(pickguard?.Nev)) titleParts.Add(pickguard.Nev);
                if (!string.IsNullOrWhiteSpace(neck?.Nev)) titleParts.Add(neck.Nev);

                var name = titleParts.Count > 0 ? string.Join(" - ", titleParts) : "Egyedi gitár";
                var price = (testforma?.Ar ?? 0) + (neck?.Ar ?? 0) + (finish?.Ar ?? 0) + (pickguard?.Ar ?? 0);
                var image = finish?.KepUrl ?? pickguard?.KepUrl ?? neck?.KepUrl ?? string.Empty;

                return new CustomGuitarDto(
                    g.Id,
                    name,
                    price,
                    image,
                    testforma?.Nev,
                    neck?.Nev,
                    finish?.Nev,
                    pickguard?.Nev,
                    finish?.KepUrl,
                    pickguard?.KepUrl,
                    neck?.KepUrl
                );
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");

            var orders = await _db.Rendelesek
                .AsNoTracking()
                .Where(r => r.FelhasznaloId == user.Id && r.Status == OrderStatus)
                .Include(r => r.Tetelek)
                .OrderByDescending(r => r.Datum)
                .ToListAsync();

            var termekIds = orders
                .SelectMany(r => r.Tetelek)
                .Where(t => t.TermekId.HasValue && t.Darabszam > 0)
                .Select(t => t.TermekId!.Value)
                .Distinct()
                .ToList();

            var customIds = orders
                .SelectMany(r => r.Tetelek)
                .Where(t => t.EgyediGitarId.HasValue && t.Darabszam > 0)
                .Select(t => t.EgyediGitarId!.Value)
                .Distinct()
                .ToList();

            var termekek = await _db.Termekek
                .AsNoTracking()
                .Include(t => t.TermekKepek)
                .Where(t => termekIds.Contains(t.Id))
                .ToListAsync();

            var termekById = termekek.ToDictionary(t => t.Id);
            var customGuitarDtos = await LoadCustomGuitarDtosAsync(customIds);

            var orderDtos = orders.Select(r => new
            {
                id = r.Id,
                datum = r.Datum,
                osszeg = r.Osszeg,
                status = r.Status,
                tetelek = r.Tetelek
                    .Where(t => t.Darabszam > 0)
                    .Select(t => new
                    {
                        id = t.Id,
                        termekId = t.TermekId,
                        egyediGitarId = t.EgyediGitarId,
                        darabszam = t.Darabszam,
                        termek = t.TermekId.HasValue && termekById.TryGetValue(t.TermekId!.Value, out var termek) ? new
                        {
                            id = termek.Id,
                            nev = termek.Nev,
                            ar = termek.Ar,
                            kep = termek.TermekKepek?.Kep1 ?? string.Empty
                        } : null,
                        customGitar = t.EgyediGitarId.HasValue && customGuitarDtos.TryGetValue(t.EgyediGitarId.Value, out var customGitar) ? customGitar : null
                    })
                    .ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        [HttpGet("{orderId:int}")]
        public async Task<IActionResult> GetOrderDetails(int orderId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return Unauthorized("Kérjük, jelentkezz be!");

            var order = await _db.Rendelesek
                .AsNoTracking()
                .Where(r => r.Id == orderId && r.FelhasznaloId == user.Id && r.Status == OrderStatus)
                .Include(r => r.Tetelek)
                .FirstOrDefaultAsync();

            if (order == null) return NotFound("Rendelés nem található");

            var termekIds = order.Tetelek
                .Where(t => t.TermekId.HasValue && t.Darabszam > 0)
                .Select(t => t.TermekId!.Value)
                .Distinct()
                .ToList();

            var customIds = order.Tetelek
                .Where(t => t.EgyediGitarId.HasValue && t.Darabszam > 0)
                .Select(t => t.EgyediGitarId!.Value)
                .Distinct()
                .ToList();

            var termekek = await _db.Termekek
                .AsNoTracking()
                .Include(t => t.TermekKepek)
                .Where(t => termekIds.Contains(t.Id))
                .ToListAsync();

            var termekById = termekek.ToDictionary(t => t.Id);
            var customGuitarDtos = await LoadCustomGuitarDtosAsync(customIds);

            var orderDetails = new
            {
                id = order.Id,
                datum = order.Datum,
                osszeg = order.Osszeg,
                status = order.Status,
                tetelek = order.Tetelek
                    .Where(t => t.Darabszam > 0)
                    .Select(t => new
                    {
                        id = t.Id,
                        termekId = t.TermekId,
                        egyediGitarId = t.EgyediGitarId,
                        darabszam = t.Darabszam,
                        termek = t.TermekId.HasValue && termekById.TryGetValue(t.TermekId.Value, out var termek) ? new
                        {
                            id = termek.Id,
                            nev = termek.Nev,
                            ar = termek.Ar,
                            kep = termek.TermekKepek?.Kep1 ?? string.Empty
                        } : null,
                        customGitar = t.EgyediGitarId.HasValue && customGuitarDtos.TryGetValue(t.EgyediGitarId.Value, out var customGitar) ? customGitar : null
                    })
                    .ToList()
            };

            return Ok(orderDetails);
        }
    }
}
