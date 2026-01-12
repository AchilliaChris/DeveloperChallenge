using DataAccess;

namespace DeveloperChallenge
{
    public class RoomBookingService : IRoomBookingService
    {
        public RoomBookingService() { }

        public bool RoomBooked(Room room, DateTime startDate, DateTime endDate)
        {
            if (room.Bookings != null)
                return room.Bookings.Any(b => startDate.Date >= b.StartDate.Date && startDate.Date <= b.EndDate.Date || endDate.Date >= b.StartDate.Date && endDate.Date <= b.EndDate.Date || b.StartDate.Date >= startDate.Date && b.StartDate.Date <= endDate.Date);
            else return false; // there are no bookings for this room
        }
    }
}
