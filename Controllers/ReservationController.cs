using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReservationController(ApplicationDbContext context) { _context = context; }

        [Authorize(Roles = "Admin,Staff")] // Only Admin/Staff can view all reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations.ToListAsync();
            return View(reservations);
        }

        public IActionResult Create() => View();

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }
        // Allow customers to view their own reservation details
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id && m.CustomerEmail == User.Identity.Name);
            if (reservation == null) return NotFound();
            return View("Details", reservation); // Use the same Details view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                reservation.Status ??= "Confirmed";
                reservation.SpecialRequests ??= "None"; // Ensures DB doesn't reject null
                
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Reservation booked successfully!";
                
                // Redirect to review page immediately so they can see what they booked
                if (User.IsInRole("Customer"))
                {
                    return RedirectToAction(nameof(CustomerDetails), new { id = reservation.Id });
                }
                return RedirectToAction(nameof(Details), new { id = reservation.Id });
            }
            return View(reservation);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}