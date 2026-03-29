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

        [AllowAnonymous]
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
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();
            order.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Receipt(int id)
        {
            var order = await _context.Orders.Include(o => o.OrderDetails)
                .ThenInclude(od => od.MenuItem).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null) return NotFound();
            return View(order);
        }
    }

}