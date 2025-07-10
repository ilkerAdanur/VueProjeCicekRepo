using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.API.Data;
using FlowerShop.API.Models;

namespace FlowerShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowersController : ControllerBase
    {
        private readonly FlowerShopContext _context;

        public FlowersController(FlowerShopContext context)
        {
            _context = context;
        }

        // GET: api/Flowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetFlowers(
            int? categoryId = null, 
            string? search = null, 
            int page = 1, 
            int pageSize = 12)
        {
            var query = _context.Flowers
                .Include(f => f.Category)
                .Where(f => f.IsActive);

            if (categoryId.HasValue)
            {
                query = query.Where(f => f.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(f => f.Name.Contains(search) || f.Description.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var flowers = await query
                .OrderBy(f => f.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Description,
                    f.Price,
                    f.ImageUrl,
                    f.Stock,
                    f.CategoryId,
                    CategoryName = f.Category.Name
                })
                .ToListAsync();

            return Ok(new
            {
                flowers,
                totalCount,
                page,
                pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
            });
        }

        // GET: api/Flowers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetFlower(int id)
        {
            var flower = await _context.Flowers
                .Include(f => f.Category)
                .Where(f => f.Id == id && f.IsActive)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Description,
                    f.Price,
                    f.ImageUrl,
                    f.Stock,
                    f.CategoryId,
                    CategoryName = f.Category.Name,
                    f.CreatedAt,
                    f.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (flower == null)
            {
                return NotFound();
            }

            return flower;
        }

        // GET: api/Flowers/featured
        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<object>>> GetFeaturedFlowers()
        {
            var flowers = await _context.Flowers
                .Include(f => f.Category)
                .Where(f => f.IsActive && f.Stock > 0)
                .OrderByDescending(f => f.CreatedAt)
                .Take(8)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Description,
                    f.Price,
                    f.ImageUrl,
                    f.Stock,
                    CategoryName = f.Category.Name
                })
                .ToListAsync();

            return Ok(flowers);
        }

        // POST: api/Flowers
        [HttpPost]
        public async Task<ActionResult<Flower>> PostFlower(Flower flower)
        {
            flower.CreatedAt = DateTime.UtcNow;
            flower.UpdatedAt = DateTime.UtcNow;
            
            _context.Flowers.Add(flower);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlower", new { id = flower.Id }, flower);
        }

        // PUT: api/Flowers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlower(int id, Flower flower)
        {
            if (id != flower.Id)
            {
                return BadRequest();
            }

            flower.UpdatedAt = DateTime.UtcNow;
            _context.Entry(flower).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Flowers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlower(int id)
        {
            var flower = await _context.Flowers.FindAsync(id);
            if (flower == null)
            {
                return NotFound();
            }

            // Soft delete
            flower.IsActive = false;
            flower.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FlowerExists(int id)
        {
            return _context.Flowers.Any(e => e.Id == id);
        }
    }
}
