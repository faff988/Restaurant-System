using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context) { _context = context; }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem).ToListAsync();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.MenuItems = await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Added for security
        public async Task<IActionResult> Create(string customerName, string customerEmail, int[]? menuItemIds, int[]? quantities)
        {
            if (menuItemIds == null || quantities == null || menuItemIds.Length == 0)
            {
                ModelState.AddModelError("", "Please select at least one menu item.");
                ViewBag.MenuItems = await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync();
                return View();
            }

            var order = new Order 
            { 
                CustomerName = customerName, 
                CustomerEmail = User.IsInRole("Customer") ? User.Identity?.Name : customerEmail,
                OrderDetails = new List<OrderDetail>()
            };

            decimal total = 0;
            
            // Batch fetch menu items for better performance
            var menuItems = await _context.MenuItems
                .Where(m => menuItemIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id);

            for (int i = 0; i < Math.Min(menuItemIds.Length, quantities.Length); i++)
            {
                if (menuItems.TryGetValue(menuItemIds[i], out var menuItem) && quantities[i] > 0)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        MenuItemId = menuItemIds[i],
                        Quantity = quantities[i],
                        UnitPrice = menuItem.Price
                    });
                    total += menuItem.Price * quantities[i];
                }
            }

            if (order.OrderDetails.Count == 0)
            {
                ModelState.AddModelError("", "Invalid quantities selected.");
                ViewBag.MenuItems = await _context.MenuItems.Where(m => m.IsAvailable).ToListAsync();
                return View();
            }

            order.TotalAmount = total;
            order.Status = "Pending";
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Order placed successfully!";
            
            if (User.IsInRole("Customer"))
            {
                return RedirectToAction(nameof(CustomerDetails), new { id = order.Id });
            }

            // Staff/Admins now see the specific order details immediately to confirm
            return RedirectToAction(nameof(Details), new { id = order.Id }); 
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }
        // Allow customers to view their own order details
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.MenuItem).FirstOrDefaultAsync(o => o.Id == id && o.CustomerEmail == User.Identity.Name);
            if (order == null) return NotFound();
            return View("Details", order); // Use the same Details view
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Receipt(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }
    }

}