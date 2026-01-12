using Microsoft.EntityFrameworkCore;

namespace HotelsAPIs
{
    public static class TestDataSeeding
    {
        public static void CleanData(string connectionString)
        {
            using (var context = new DataAccess.HotelsDbContext(
                new DbContextOptionsBuilder<DataAccess.HotelsDbContext>()
                .UseSqlServer(connectionString)
                .Options))
            {
                context.Database.ExecuteSqlRaw("ALTER TABLE Rooms Drop Constraint FK_Rooms_Hotels_HotelId");
                context.Database.ExecuteSqlRaw("ALTER TABLE RoomBookings Drop Constraint FK_RoomBookings_Rooms_RoomId");
                context.Database.ExecuteSqlRaw("ALTER TABLE RoomBookings Drop Constraint FK_RoomBookings_Bookings_BookingId");
                context.Database.ExecuteSqlRaw("ALTER TABLE Bookings Drop Constraint FK_Bookings_Customers_CustomerId");
                context.Database.ExecuteSqlRaw("ALTER TABLE GuestBookings Drop Constraint FK_GuestBookings_RoomBookings_RoomBookingId");
                context.Database.ExecuteSqlRaw("ALTER TABLE GuestBookings Drop Constraint FK_GuestBookings_Customers_GuestId");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE Payments");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE RoomBookings");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE GuestBookings");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE Bookings");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE Customers");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE Rooms");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE Hotels");
                context.Database.ExecuteSqlRaw("ALTER TABLE Rooms Add Constraint FK_Rooms_Hotels_HotelId Foreign Key (HotelId) references Hotels(Id) ON DELETE CASCADE");
                context.Database.ExecuteSqlRaw("ALTER TABLE RoomBookings Add Constraint FK_RoomBookings_Rooms_RoomId Foreign Key (RoomId) references Rooms(Id) ON DELETE CASCADE");
                context.Database.ExecuteSqlRaw("ALTER TABLE RoomBookings Add Constraint FK_RoomBookings_Bookings_BookingId Foreign Key (BookingId) references Bookings(Id) ON DELETE CASCADE");
                context.Database.ExecuteSqlRaw("ALTER TABLE Bookings Add Constraint FK_Bookings_Customers_CustomerId Foreign Key ( CustomerId ) references Customers(Id)  ON DELETE CASCADE");
                context.Database.ExecuteSqlRaw("ALTER TABLE GuestBookings Add Constraint FK_GuestBookings_RoomBookings_RoomBookingId Foreign Key (RoomBookingId) references RoomBookings(Id) ON DELETE CASCADE");
                context.Database.ExecuteSqlRaw("ALTER TABLE GuestBookings Add Constraint FK_GuestBookings_Customers_GuestId Foreign Key (GuestId) references Customers(Id) ON DELETE NO ACTION");
            }
        }

        public static async Task RefreshData(string connectionString)
        {
            CleanData(connectionString);
            using (var context = new DataAccess.HotelsDbContext(
                new DbContextOptionsBuilder<DataAccess.HotelsDbContext>()
                .UseSqlServer(connectionString)
                .Options))
            {
                await context.Hotels.AddRangeAsync(
                      new DataAccess.Hotel { Name = "Grand Plaza", Address = "123 Main St, Cityville", Phone = "+44 1234 56789123" },
                      new DataAccess.Hotel { Name = "Mardon Villa", Address = "28 High St, Redtown", Phone = "+44 1417 9258465" },
                      new DataAccess.Hotel { Name = "Hilton Heights", Address = "425 Main Rd, Bluefield", Phone = "+44 1187 62549785" }
                      );
                await context.SaveChangesAsync();

                await context.Customers.AddRangeAsync(
                         new DataAccess.Customer { FirstName = "John", LastName = "Doe", Address = "456 Elm St, Townsville", Email = "jdoe@highdon.com", Phone = "+44 1294 567890" },
                         new DataAccess.Customer { FirstName = "Hayley", LastName = "Tilsley", Address = "9 random Way, Middlebridge", Email = "htilsley@outlook.co.uk", Phone = "+44 1934 3451915" },
                         new DataAccess.Customer { FirstName = "Rachel", LastName = "Piemaker", Address = "45 Least Road, Kettleborough", Email = "rpiemaker@gmail.com", Phone = "+44 1454 9427584" },
                         new DataAccess.Customer { FirstName = "Paul", LastName = "Pope", Address = "91 Rude Avenue, Greatley", Email = "ppope@futuremail.co.uk", Phone = "+44 1917 2365548" },
                         new DataAccess.Customer { FirstName = "Jane", LastName = "Carter", Address = "75 Bell View, Hartlingshine", Email = "jcarter@gmail.com", Phone = "+44 1652 354584" }
                         );
                await context.SaveChangesAsync();

                await context.Rooms.AddRangeAsync(
                    new DataAccess.Room { HotelId = 1, RoomTypeId = 1, RoomNumber = 1, PricePerNight = 75.00, Capacity = 1 },
                    new DataAccess.Room { HotelId = 1, RoomTypeId = 2, RoomNumber = 2, PricePerNight = 155.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 1, RoomTypeId = 2, RoomNumber = 3, PricePerNight = 150.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 1, RoomTypeId = 3, RoomNumber = 4, PricePerNight = 175.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 1, RoomTypeId = 2, RoomNumber = 5, PricePerNight = 150.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 1, RoomTypeId = 3, RoomNumber = 6, PricePerNight = 175.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 2, RoomTypeId = 1, RoomNumber = 1, PricePerNight = 75.00, Capacity = 1 },
                    new DataAccess.Room { HotelId = 2, RoomTypeId = 1, RoomNumber = 2, PricePerNight = 75.00, Capacity = 1 },
                    new DataAccess.Room { HotelId = 2, RoomTypeId = 2, RoomNumber = 3, PricePerNight = 250.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 2, RoomTypeId = 1, RoomNumber = 4, PricePerNight = 75.00, Capacity = 1 },
                    new DataAccess.Room { HotelId = 2, RoomTypeId = 2, RoomNumber = 5, PricePerNight = 250.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 2, RoomTypeId = 2, RoomNumber = 6, PricePerNight = 250.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 3, RoomTypeId = 3, RoomNumber = 1, PricePerNight = 250.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 3, RoomTypeId = 1, RoomNumber = 2, PricePerNight = 175.00, Capacity = 1 },
                    new DataAccess.Room { HotelId = 3, RoomTypeId = 3, RoomNumber = 3, PricePerNight = 275.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 3, RoomTypeId = 3, RoomNumber = 4, PricePerNight = 275.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 3, RoomTypeId = 3, RoomNumber = 5, PricePerNight = 275.00, Capacity = 2 },
                    new DataAccess.Room { HotelId = 3, RoomTypeId = 3, RoomNumber = 6, PricePerNight = 275.00, Capacity = 2 }
                    );
                await context.SaveChangesAsync();

                await context.Bookings.AddRangeAsync(
                    new DataAccess.Booking { CustomerId = 1, BookingReference = "PrhEjxxuk1Bnp", TotalPrice = 475, Cancelled = false },
                    new DataAccess.Booking { CustomerId = 2, BookingReference = "Z26UtejKnmWtA", TotalPrice = 280, Cancelled = false },
                    new DataAccess.Booking { CustomerId = 3, BookingReference = "XR1NHc5U9Fl74", TotalPrice = 1450, Cancelled = false }
                    );
                await context.SaveChangesAsync();

                await context.RoomBookings.AddRangeAsync(
                    new DataAccess.RoomBooking { BookingId = 1, RoomId = 2, StartDate = new System.DateTime(2026, 7, 1), EndDate = new System.DateTime(2026, 7, 5) },
                    new DataAccess.RoomBooking { BookingId = 2, RoomId = 3, StartDate = new System.DateTime(2026, 8, 10), EndDate = new System.DateTime(2026, 8, 15) },
                    new DataAccess.RoomBooking { BookingId = 3, RoomId = 4, StartDate = new System.DateTime(2026, 9, 20), EndDate = new System.DateTime(2026, 9, 25) },
                    new DataAccess.RoomBooking { BookingId = 1, RoomId = 3, StartDate = new System.DateTime(2026, 7, 1), EndDate = new System.DateTime(2026, 7, 5) },
                        new DataAccess.RoomBooking { BookingId = 2, RoomId = 4, StartDate = new System.DateTime(2026, 8, 10), EndDate = new System.DateTime(2026, 8, 15) },
                        new DataAccess.RoomBooking { BookingId = 3, RoomId = 5, StartDate = new System.DateTime(2026, 9, 20), EndDate = new System.DateTime(2026, 9, 25) }
                    );
                await context.SaveChangesAsync();

                await context.GuestBookings.AddRangeAsync(
                    new DataAccess.GuestBooking { RoomBookingId = 1, GuestId = 1 },
                    new DataAccess.GuestBooking { RoomBookingId = 2, GuestId = 2 },
                    new DataAccess.GuestBooking { RoomBookingId = 3, GuestId = 3 },
                    new DataAccess.GuestBooking { RoomBookingId = 4, GuestId = 4 },
                    new DataAccess.GuestBooking { RoomBookingId = 5, GuestId = 5 },
                    new DataAccess.GuestBooking { RoomBookingId = 6, GuestId = 1 }
                    );

                await context.SaveChangesAsync();
            }


        }
    }
}
