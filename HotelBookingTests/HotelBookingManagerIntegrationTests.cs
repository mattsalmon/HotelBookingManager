using System;
using HotelBookingDomain;
using HotelBookingData;
using Xunit;

namespace HotelBookingTests
{
    public class HotelBookingManagerIntegrationTests : IDisposable
    {
        private IHotelBookingRepository _dataRepository;
        
        public HotelBookingManagerIntegrationTests()
        {
            _dataRepository = new InMemoryBookingRepository();
        }
        [Fact]
        [Trait("Category","Integration")]
        public void WhenRoomHasNoBooking_VerifyRoomIsAvailable()
        {
            var roomToBeBooked = 101;
            var dateOfBooking = new DateTime(2012,3,28);

            var bookingManager = new BookingManager(_dataRepository);

            Assert.True(bookingManager.IsRoomAvailable(roomToBeBooked, dateOfBooking)); // outputs true
        }

        [Fact]
        [Trait("Category","Integration")]
        public void WhenRoomHasBeenBooked_VerifyRoomIsNotAvailable()
        {
            var surname = "Smith";
            var roomToBeBooked = 201;
            var dateOfBooking = new DateTime(2012,3,28);

            var bookingManager = new BookingManager(_dataRepository);

            bookingManager.AddBooking(surname, roomToBeBooked, dateOfBooking);
            Assert.False(bookingManager.IsRoomAvailable(roomToBeBooked, dateOfBooking));
        }
        [Fact]
        [Trait("Category","Integration")]
        public void WhenRoomIsDoubleBooked_VerifyExceptionThrown()
        {
            var today = new DateTime(2012,3,28);
            var bookingManager = new BookingManager(_dataRepository);
            bookingManager.AddBooking("Patel", 101, today);
            Assert.Throws<RoomNotAvailableException>(() => bookingManager.AddBooking("Li", 101, today));
        }

        public void Dispose()
        {
            _dataRepository.Dispose();
        }
    }
}