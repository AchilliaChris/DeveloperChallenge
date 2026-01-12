using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DeveloperChallenge
{
    public class HotelService : IHotelService
    {
        private readonly HotelsDbContext context;
        private readonly IRoomBookingService roomBookingService;
        public HotelService(HotelsDbContext _context,
            IRoomBookingService _roomBookingService)
        {
            context = _context;
            roomBookingService = _roomBookingService;
        }

        public async Task<List<Hotel>> GetHotelByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Hotel name cannot be null or empty.", nameof(name));
            }

            if (name.Length < 3)
            {
                throw new ArgumentException("Hotel name must be at least 3 characters long.", nameof(name));
            }

            if (context.Hotels.Any(h => h.Name.ToLower().Equals(name.ToLower())))
            {
                return context.Hotels.Include(h => h.Rooms).Where(h => h.Name.ToLower().Equals(name.ToLower())).ToList();
            }
            else
            {
                return new List<Hotel>(); // we could look at ranking the available hotels and returning top 5
            }

        }
        public async Task<List<Hotel>> GetAvailableHotelRooms(DateTime startDate, DateTime endDate, int numberOfGuests)
        {
            var AvailableHotels = await context.Hotels
                 .Include(h => h.Rooms)
                 .ThenInclude(r => r.Bookings)
                 .AsAsyncEnumerable()
                 .Where(h => h.Rooms.Any(r => !roomBookingService.RoomBooked(r, startDate, endDate)))
                 .ToListAsync();

            foreach (var hotel in AvailableHotels)
            {
                hotel.Rooms = hotel.Rooms.Where(r => ! roomBookingService.RoomBooked(r, startDate, endDate)).ToList();
            }

           return  AvailableHotels.Where(h => h.Rooms.Sum(r => r.Capacity) >= numberOfGuests).ToList();

        }
    }
}
