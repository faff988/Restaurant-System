namespace RestaurantSystem.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public DateTime ReservationDate { get; set; }
        public int NumberOfGuests { get; set; }
        public string Status { get; set; } = "Confirmed";
        public string SpecialRequests { get; set; } = string.Empty;
    }
}