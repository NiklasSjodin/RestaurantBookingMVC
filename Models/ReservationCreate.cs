using Newtonsoft.Json;

namespace RestaurantBookingMVC.Models
{
    public class ReservationCreate
    {
        public int TableID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Time { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
