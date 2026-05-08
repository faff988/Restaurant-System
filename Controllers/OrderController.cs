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
        public async Task<IActionResult> Create(string customerName, string customerEmail, int[] menuItemIds, int[] quantities)
        {
            var order = new Order { CustomerName = customerName, CustomerEmail = customerEmail };
            decimal total = 0;

            for (int i = 0; i < menuItemIds.Length; i++)
            {
                var menuItem = await _context.MenuItems.FindAsync(menuItemIds[i]);
                if (menuItem != null && quantities[i] > 0)
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

            order.TotalAmount = total;
            order.Status = "Pending";
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Order placed successfully!";
            
            // Unified Flow: Staff and Admins see the management list immediately
            if (User.IsInRole("Admin") || User.IsInRole("Staff"))
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
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