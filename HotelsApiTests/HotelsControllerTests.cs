using AutoMapper;
using DataAccess;
using DeveloperChallenge;
using DeveloperChallenge.ViewModels;
using HotelsAPIs.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;


namespace HotelsApiTests
{
    public class HotelsControllerTests
    {
        private static HotelsController CreateController(
            Mock<IHotelService>? hotelService = null,
            Mock<IMapper>? mapper = null)
        {
            var hs = hotelService ?? new Mock<IHotelService>();
            var mp = mapper ?? new Mock<IMapper>();
            var logger = NullLogger<HotelsController>.Instance;
            return new HotelsController(hs.Object, logger, mp.Object);
        }

        [Fact]
        public async Task GetHotelByName_ReturnsMappedHotels()
        {
            // Arrange
            var name = "Hilton";
            var hotelEntities = new List<Hotel>
            {
                new Hotel { HotelId = 1, Name = "Hilton Downtown", Address = "A", Phone = "P" },
                new Hotel { HotelId = 2, Name = "Hilton Uptown", Address = "B", Phone = "Q" }
            };

            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(name))
                .ReturnsAsync(hotelEntities);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<List<Hotel>, List<HotelViewModel>>(It.IsAny<List<Hotel>>()))
                .Returns<List<Hotel>>(src => src.Select(h => new HotelViewModel { Name = h.Name }).ToList());

            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act
            var result = await controller.GetHotelByName(name);

            // Assert
            var array = result.ToArray();
            Assert.Equal(2, array.Length);
            Assert.Contains(array, h => h.Name == "Hilton Downtown");
            Assert.Contains(array, h => h.Name == "Hilton Uptown");
        }

        [Fact]
        public async Task GetHotelByName_WhenServiceReturnsEmpty_ReturnsEmptyArray()
        {
            // Arrange
            var name = "NoSuchHotel";
            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(name))
                .ReturnsAsync(new List<Hotel>());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<List<Hotel>, List<HotelViewModel>>(It.IsAny<List<Hotel>>()))
                .Returns(new List<HotelViewModel>());

            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act
            var result = await controller.GetHotelByName(name);

            // Assert
            var array = result.ToArray();
            Assert.Empty(array);
        }

        [Fact]
        public async Task GetHotelByName_PassesNameToService()
        {
            // Arrange
            var name = "ExactMatchName";
            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(It.IsAny<string>()))
                .ReturnsAsync(new List<Hotel>());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<List<Hotel>, List<HotelViewModel>>(It.IsAny<List<Hotel>>()))
                .Returns(new List<HotelViewModel>());

            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act
            var _ = await controller.GetHotelByName(name);

            // Assert
            hotelServiceMock.Verify(s => s.GetHotelByName(name), Times.Once);
        }

        [Fact]
        public async Task GetHotelByName_WhenNameIsNull_ForwardsNullToService()
        {
            // Arrange
            string? name = null;
            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(null!))
                .ReturnsAsync(new List<Hotel>());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<List<Hotel>, List<HotelViewModel>>(It.IsAny<List<Hotel>>()))
                .Returns(new List<HotelViewModel>());

            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act
            var _ = await controller.GetHotelByName(name!);

            // Assert
            hotelServiceMock.Verify(s => s.GetHotelByName(null!), Times.Once);
        }

        [Fact]
        public async Task GetHotelByName_WhenServiceThrows_ExceptionPropagates()
        {
            // Arrange
            var name = "boom";
            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(It.IsAny<string>()))
                .ThrowsAsync(new InvalidOperationException("svc-failed"));

            var mapperMock = new Mock<IMapper>();
            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act / Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await controller.GetHotelByName(name));
        }

        [Fact]
        public async Task GetHotelByName_WhenMapperReturnsNull_ThrowsNullReferenceException()
        {
            // Arrange
            var name = "some";
            var hotelEntities = new List<Hotel> { new Hotel { HotelId = 1, Name = "H" } };

            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(It.IsAny<string>()))
                .ReturnsAsync(hotelEntities);

            var mapperMock = new Mock<IMapper>();
            // mapper returns null which will cause .ToArray() to throw
            mapperMock
                .Setup(m => m.Map<List<Hotel>, List<HotelViewModel>>(It.IsAny<List<Hotel>>()))
                .Returns((List<HotelViewModel>?)null!);

            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act / Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () => await controller.GetHotelByName(name));
        }

        [Fact]
        public async Task GetHotelByName_WhenMapperThrows_ExceptionPropagates()
        {
            // Arrange
            var name = "mapfail";
            var hotelServiceMock = new Mock<IHotelService>();
            hotelServiceMock
                .Setup(s => s.GetHotelByName(It.IsAny<string>()))
                .ReturnsAsync(new List<Hotel>());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<List<Hotel>, List<HotelViewModel>>(It.IsAny<List<Hotel>>()))
                .Throws(new ApplicationException("mapping failed"));

            var controller = CreateController(hotelServiceMock, mapperMock);

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(async () => await controller.GetHotelByName(name));
        }
        }
}
