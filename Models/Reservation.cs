using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RestaurantSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Reservation Date")]
        public DateTime ReservationDate { get; set; }

        [Required]
        [Range(1, 20, ErrorMessage = "Please enter a value between 1 and 20")]
        [Display(Name = "Guests")]
        public int NumberOfGuests { get; set; }

        public string Status { get; set; } = "Confirmed";

        [Display(Name = "Special Requests")]
        public string? SpecialRequests { get; set; }

        // Links the reservation to the registered User
        public string? UserId { get; set; }
        public virtual IdentityUser? User { get; set; }
    }
}