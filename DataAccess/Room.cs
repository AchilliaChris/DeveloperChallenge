namespace DataAccess
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public int RoomNumber { get; set; }
        public double PricePerNight { get; set; }
        public int Capacity { get; set; } = 0;
        public List<RoomBooking> Bookings { get; set; }
        public Hotel Hotel { get; set; }
    }
}
