using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Data;
using RestaurantSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace RestaurantSystem.Controllers
{
    [Authorize]
    public class ReservationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,Staff")] // Only Admin/Staff can view all reservations
        public async Task<IActionResult> Index()
        {
            var reservations = await _context.Reservations.ToListAsync();
            return View(reservations);
        }

        public async Task<IActionResult> Create()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = await _userManager.GetUserAsync(User);
                var reservation = new Reservation
                {
                    CustomerEmail = user?.Email ?? "",
                    CustomerName = user?.UserName?.Split('@')[0] ?? "" // Guesses a name from the email
                };
                return View(reservation);
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var reservation = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id);
            if (reservation == null) return NotFound();
            return View(reservation);
        }
        // Allow customers to view their own reservation details
        // This action is for customers to view THEIR OWN reservations.
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerDetails(int id)
        {
            var userId = _userManager.GetUserId(User);
            var reservation = await _context.Reservations.FirstOrDefaultAsync(m => m.Id == id && (m.UserId == userId || m.CustomerEmail == User.Identity.Name));
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
                
                // Link the reservation to the current logged-in user
                reservation.UserId = _userManager.GetUserId(User);
                
                try
                {
                    _context.Reservations.Add(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Database Error: Please ensure you have run 'dotnet ef database update'. Details: " + ex.InnerException?.Message);
                    return View(reservation);
                }

                TempData["Success"] = "Reservation booked successfully!";
                
                if (User.IsInRole("Customer"))
                {
                    return RedirectToAction(nameof(CustomerDetails), new { id = reservation.Id });
                }
                
                // Staff/Admins see the specific booking details immediately
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