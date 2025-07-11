using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.API.Data;

namespace FlowerShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly FlowerShopContext _context;

        public DashboardController(FlowerShopContext context)
        {
            _context = context;
        }

        // GET: api/Dashboard/stats
        [HttpGet("stats")]
        public async Task<ActionResult<object>> GetStats()
        {
            try
            {
                var totalFlowers = await _context.Flowers.CountAsync(f => f.IsActive);
                var totalCategories = await _context.Categories.CountAsync(c => c.IsActive);
                var totalOrders = await _context.Orders.CountAsync();
                var pendingOrders = await _context.Orders.CountAsync(o => o.Status == Models.OrderStatus.Pending);

                return Ok(new
                {
                    totalFlowers,
                    totalCategories,
                    totalOrders,
                    pendingOrders
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error loading dashboard stats", error = ex.Message });
            }
        }
    }
}
