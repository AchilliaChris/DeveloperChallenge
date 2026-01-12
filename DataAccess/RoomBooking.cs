namespace DataAccess
{
   public class RoomBooking
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Booking Booking { get; set; } = null!;
        public Room Room { get; set; } = null!;
        public ICollection<GuestBooking> GuestBookings { get; set; } = null!;
    }
}
