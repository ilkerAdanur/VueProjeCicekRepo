using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.API.Data;
using FlowerShop.API.Models;

namespace FlowerShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OccasionsController : ControllerBase
    {
        private readonly FlowerShopContext _context;

        public OccasionsController(FlowerShopContext context)
        {
            _context = context;
        }

        // GET: api/Occasions
        [HttpGet]
        public async Task<ActionResult<object>> GetOccasions()
        {
            var occasions = await _context.Occasions
                .Where(o => o.IsActive)
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.Description,
                    o.Icon,
                    FlowerCount = o.Flowers.Count(f => f.IsActive)
                })
                .OrderBy(o => o.Name)
                .ToListAsync();

            return new { occasions };
        }

        // GET: api/Occasions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOccasion(int id)
        {
            var occasion = await _context.Occasions
                .Where(o => o.Id == id && o.IsActive)
                .Select(o => new
                {
                    o.Id,
                    o.Name,
                    o.Description,
                    o.Icon,
                    Flowers = o.Flowers
                        .Where(f => f.IsActive)
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
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (occasion == null)
            {
                return NotFound();
            }

            return occasion;
        }

        // GET: api/Occasions/5/flowers
        [HttpGet("{id}/flowers")]
        public async Task<ActionResult<object>> GetOccasionFlowers(int id)
        {
            var occasion = await _context.Occasions
                .Where(o => o.Id == id && o.IsActive)
                .FirstOrDefaultAsync();

            if (occasion == null)
            {
                return NotFound();
            }

            var flowers = await _context.Flowers
                .Where(f => f.OccasionId == id && f.IsActive)
                .Select(f => new
                {
                    f.Id,
                    f.Name,
                    f.Description,
                    f.Price,
                    f.ImageUrl,
                    f.Stock,
                    CategoryName = f.Category.Name,
                    OccasionName = f.Occasion!.Name
                })
                .OrderBy(f => f.Name)
                .ToListAsync();

            return new { 
                occasion = new { occasion.Id, occasion.Name, occasion.Description, occasion.Icon },
                flowers 
            };
        }
    }
}
