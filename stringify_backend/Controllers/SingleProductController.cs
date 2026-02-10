using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using stringify_backend.Dtos;
using stringify_backend.Models;

namespace Stringify.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class SingleProductController : ControllerBase
    {
        private readonly StringifyDbContext _db;

        public SingleProductController(StringifyDbContext db)
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

                    Images = new List<string>
                    {
                        t.TermekKepek != null ? t.TermekKepek.Kep1 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep2 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep3 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep4 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep5 : ""
                    },

                    Rating = null,
                    ReviewCount = null
                })
                .ToListAsync();

            // Filter empty images AFTER retrieving from database
            foreach (var product in products)
            {
                product.Images = product.Images
                    .Where(url => !string.IsNullOrWhiteSpace(url))
                    .ToList();
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var product = await _db.Termekek
                .AsNoTracking()
                .Include(t => t.TermekKepek)
                .Where(t => t.Id == id)
                .Select(t => new ProductDto
                {
                    Id = t.Id,
                    Title = t.Nev,
                    ShortDescription = t.RovidLeiras,
                    LongDescription = t.Leiras,
                    PreviewDescription = t.RovidLeiras ?? "",

                    Price = t.Ar,
                    IsAvailable = t.Elerheto,

                    Images = new List<string>
                    {
                        t.TermekKepek != null ? t.TermekKepek.Kep1 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep2 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep3 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep4 : "",
                        t.TermekKepek != null ? t.TermekKepek.Kep5 : ""
                    },

                    Rating = null,
                    ReviewCount = null
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            // Filter empty images
            product.Images = product.Images
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .ToList();

            return Ok(product);
        }
    }
}