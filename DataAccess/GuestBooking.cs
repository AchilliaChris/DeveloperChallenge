namespace DataAccess
{
    public class GuestBooking
    {
        public int Id { get; set; }
        public int RoomBookingId { get; set; }
        public int GuestId { get; set; }
        public RoomBooking RoomBooking { get; set; } = null!;
        public Customer Guest { get; set; } = null!;
    }
}
