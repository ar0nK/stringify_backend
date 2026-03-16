using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Dtos;
using stringify_backend.Models;

namespace Stringify.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StringifyDbContext _db;

        public ProductsController(StringifyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll()
        {
            var products = await _db.Termekek
                .AsNoTracking()
                .Include(t => t.TermekKepek)
                .OrderByDescending(t => t.Letrehozva)
                .Select(t => new ProductDto
                {
                    Id = t.Id,
                    Title = t.Nev,
                    ShortDescription = t.RovidLeiras,
                    LongDescription = t.Leiras,
                    PreviewDescription = t.RovidLeiras ?? "",
                    Price = t.Ar,
                    IsAvailable = t.Elerheto,
                    GuitarTypeId = t.GitarTipusId,
                    CreatedAt = t.Letrehozva,
                    Images = new List<string>
                    {
                        t.TermekKepek != null ? t.TermekKepek.Kep1 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep2 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep3 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep4 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep5 : ""
                    }
                })
                .ToListAsync();

            foreach (var product in products)
            {
                product.Images = product.Images
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .ToList();
            }

            return Ok(products);
        }
    }
}
