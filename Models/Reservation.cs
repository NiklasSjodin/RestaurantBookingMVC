﻿namespace RestaurantBookingMVC.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public DateTime Time { get; set; }
        public int NumberOfGuests { get; set; }
    }
}
