using DataAccess;

namespace DeveloperChallenge
{
    public interface IRoomBookingService
    {
        bool RoomBooked(Room room, DateTime startDate, DateTime endDate);
    }
}