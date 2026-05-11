using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RestaurantSystem.Models
{
    public class Reservation : IValidatableObject
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ReservationDate < DateTime.Now)
            {
                yield return new ValidationResult("Reservation date must be in the future.", new[] { nameof(ReservationDate) });
            }
        }
    }
}