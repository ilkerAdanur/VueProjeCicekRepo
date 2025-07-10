using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.API.Data;
using FlowerShop.API.Models;

namespace FlowerShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly FlowerShopContext _context;

        public OrdersController(FlowerShopContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Flower)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    o.DeliveryDate,
                    Customer = new
                    {
                        o.Customer.FirstName,
                        o.Customer.LastName,
                        o.Customer.Email,
                        o.Customer.Phone
                    },
                    ItemCount = o.OrderItems.Count
                })
                .ToListAsync();

            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Flower)
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.OrderDate,
                    o.TotalAmount,
                    o.Status,
                    o.DeliveryAddress,
                    o.DeliveryCity,
                    o.DeliveryPostalCode,
                    o.DeliveryDate,
                    o.Notes,
                    Customer = new
                    {
                        o.Customer.Id,
                        o.Customer.FirstName,
                        o.Customer.LastName,
                        o.Customer.Email,
                        o.Customer.Phone,
                        o.Customer.Address,
                        o.Customer.City,
                        o.Customer.PostalCode
                    },
                    OrderItems = o.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        oi.Quantity,
                        oi.UnitPrice,
                        oi.TotalPrice,
                        Flower = new
                        {
                            oi.Flower.Id,
                            oi.Flower.Name,
                            oi.Flower.ImageUrl
                        }
                    })
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<object>> PostOrder(CreateOrderRequest request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                // Create or get customer
                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == request.Customer.Email);

                if (customer == null)
                {
                    customer = new Customer
                    {
                        FirstName = request.Customer.FirstName,
                        LastName = request.Customer.LastName,
                        Email = request.Customer.Email,
                        Phone = request.Customer.Phone,
                        Address = request.Customer.Address,
                        City = request.Customer.City,
                        PostalCode = request.Customer.PostalCode
                    };
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                }

                // Generate order number
                var orderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{DateTime.Now.Ticks % 10000:D4}";

                // Create order
                var order = new Order
                {
                    OrderNumber = orderNumber,
                    CustomerId = customer.Id,
                    TotalAmount = 0, // Will be calculated
                    DeliveryAddress = request.DeliveryAddress,
                    DeliveryCity = request.DeliveryCity,
                    DeliveryPostalCode = request.DeliveryPostalCode,
                    DeliveryDate = request.DeliveryDate,
                    Notes = request.Notes
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order items and calculate total
                decimal totalAmount = 0;
                foreach (var item in request.Items)
                {
                    var flower = await _context.Flowers.FindAsync(item.FlowerId);
                    if (flower == null || flower.Stock < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for flower {item.FlowerId}");
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        FlowerId = item.FlowerId,
                        Quantity = item.Quantity,
                        UnitPrice = flower.Price
                    };

                    _context.OrderItems.Add(orderItem);
                    
                    // Update stock
                    flower.Stock -= item.Quantity;
                    flower.UpdatedAt = DateTime.UtcNow;
                    
                    totalAmount += orderItem.TotalPrice;
                }

                // Update order total
                order.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return CreatedAtAction("GetOrder", new { id = order.Id }, new { order.Id, order.OrderNumber });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // PUT: api/Orders/5/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            if (status == OrderStatus.Delivered)
            {
                order.DeliveryDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }

    public class CreateOrderRequest
    {
        public CustomerRequest Customer { get; set; } = null!;
        public List<OrderItemRequest> Items { get; set; } = new();
        public string DeliveryAddress { get; set; } = string.Empty;
        public string DeliveryCity { get; set; } = string.Empty;
        public string DeliveryPostalCode { get; set; } = string.Empty;
        public DateTime? DeliveryDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class CustomerRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }

    public class OrderItemRequest
    {
        public int FlowerId { get; set; }
        public int Quantity { get; set; }
    }
}
