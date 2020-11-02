using System;
using System.Linq;
using HotelBookingDomain;
using HotelBookingData;
using Moq;
using Xunit;

namespace HotelBookingTests
{
    public class HotelBookingManagerUnitTests: IDisposable
    {
        private Mock<IHotelBookingRepository> _mockRepository;
        public HotelBookingManagerUnitTests()
        {
            // Use the constructor to set up test context for each test to avoid repeated code
            _mockRepository = new Mock<IHotelBookingRepository>();
            
            // Set up mock data for returned booking
            var roomToBeBooked = 201;
            var dateOfBooking = DateTime.Now.NextDayOfWeek(DayOfWeek.Saturday);
            
            // Let's say that room 201 is always booked on Saturdays
            // Under all other conditions, the GetBooking method will return null (signifying no booking record)
            _mockRepository.Setup(repo => repo.GetBooking(
                It.Is<int>(i=>i==roomToBeBooked),
                It.Is<DateTime>(date => date.DayOfWeek == DayOfWeek.Saturday)))
                .Returns(()=>
                    {
                        var mockHotelBooking = new Mock<IHotelBooking>();
                        mockHotelBooking.Setup(b => b.Surname).Returns("Smith");
                        mockHotelBooking.Setup(b => b.RoomNumber).Returns(roomToBeBooked);
                        mockHotelBooking.Setup(b => b.BookingDate).Returns(dateOfBooking);
                        return mockHotelBooking.Object;
                    }
                );
            
            // The mock repository should return an successful result when there's no existing booking (Sundays are quiet days)
            _mockRepository.Setup(repo => repo.CreateBooking(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.Is<DateTime>(date => date.DayOfWeek == DayOfWeek.Sunday)))
                .Returns(()=>
                    {
                        var bookingResult = new Mock<IHotelBookingResult>();
                        bookingResult.Setup(r => r.OperationSuccessful).Returns(true);
                        bookingResult.Setup(r => r.ReasonCode).Returns("CREATED");
                        return bookingResult.Object;
                    }
                );

            // The mock repository should return an unsuccessful result when trying to double-book (Saturdays are busy)
            _mockRepository.Setup(repo => repo.CreateBooking(
                It.IsAny<string>(),
                It.Is<int>(i=>i==roomToBeBooked),
                It.Is<DateTime>(date => date.DayOfWeek == DayOfWeek.Saturday)))
                .Returns(()=>
                    {
                        var bookingResult = new Mock<IHotelBookingResult>();
                        bookingResult.Setup(r => r.OperationSuccessful).Returns(false);
                        bookingResult.Setup(r => r.ReasonCode).Returns("DUPLICATE");
                        return bookingResult.Object;
                    }
                );
        }

        public void Dispose()
        {
            _mockRepository.Object.Dispose();
        }

        [Fact]
        public void WhenRoomBookingExists_VerifyIsUnavailable()
        {
            // Mock data layer is configured to have a booking in room 201 on Saturdays
            var roomToBeBooked = 201;
            var dateOfBooking = DateTime.Now.NextDayOfWeek(DayOfWeek.Saturday);
            
            // Instantiate code under test using mock repo
            var bookingManager = new BookingManager(_mockRepository.Object);
            
            Assert.False(bookingManager.IsRoomAvailable(roomToBeBooked, dateOfBooking));
        }

        [Fact]
        public void WhenNoRoomBooking_VerifyIsAvailable()
        {
            // Mock data layer is configured not to have a room booking in 201 on Mondays
            var roomToBeBooked = 201;
            var dateOfBooking = DateTime.Now.NextDayOfWeek(DayOfWeek.Sunday);

            // Instantiate code under test using mock repo
            var bookingManager = new BookingManager(_mockRepository.Object);

            Assert.True(bookingManager.IsRoomAvailable(roomToBeBooked, dateOfBooking));
        }

        [Fact]
        public void WhenRoomBookingExists_NewBookingThrowsException()
        {
            // Mock data layer is configured to have a booking in room 201 on Saturdays
            var nameToBook = "Smith";
            var roomToBeBooked = 201;
            var dateOfBooking =  DateTime.Now.NextDayOfWeek(DayOfWeek.Saturday);

            // Instantiate code under test using mock repo
            var bookingManager = new BookingManager(_mockRepository.Object);

            Assert.Throws<RoomNotAvailableException>(() => bookingManager.AddBooking(nameToBook, roomToBeBooked, dateOfBooking));
        }

        [Fact]
        public void WhenNoRoomBookingExists_NewBookingIsSuccessful()
        {
            // Mock data layer is configured to have no bookings in room 300 on Sundays
            var nameToBook = "Smith";
            var roomToBeBooked = 300;
            var dateOfBooking = DateTime.Now.NextDayOfWeek(DayOfWeek.Sunday);

            // Instantiate code under test using mock repo
            var bookingManager = new BookingManager(_mockRepository.Object);
            bookingManager.AddBooking(nameToBook, roomToBeBooked, dateOfBooking);
            _mockRepository.Verify(mock => mock.CreateBooking(nameToBook, roomToBeBooked, dateOfBooking), Times.Exactly(1));
        }

        [Theory]
        [InlineData(DayOfWeek.Monday, 5)]
        [InlineData(DayOfWeek.Tuesday, 4)]
        [InlineData(DayOfWeek.Wednesday, 3)]
        [InlineData(DayOfWeek.Thursday, 2)]
        [InlineData(DayOfWeek.Friday, 1)]
        public void WhenNoRoomBookingExists_AvailabilityCheckReturnsCorrectRooms(DayOfWeek dayToCheckAvailability, int targetNumberOfRoomsAvailable)
        {
            // Instantiate code under test using mock repo
            var bookingManager = new BookingManager(_mockRepository.Object);
            var actualRoomsAvailable = bookingManager.getAvailableRooms(DateTime.Now.NextDayOfWeek(dayToCheckAvailability));
            
            Assert.Equal(actualRoomsAvailable.Count(), targetNumberOfRoomsAvailable);
        }
    }
}