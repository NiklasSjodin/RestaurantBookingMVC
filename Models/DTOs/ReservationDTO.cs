namespace RestaurantBookingMVC.Models.DTOs
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public DateTime Time { get; set; }
        public int TableNumber { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
