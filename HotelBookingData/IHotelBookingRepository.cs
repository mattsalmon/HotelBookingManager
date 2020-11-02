using System;
using System.Collections.Generic;

namespace HotelBookingData
{
    public interface IHotelBookingRepository: IDisposable
    {
        IHotelBookingResult CreateBooking(string surname, int roomNumber, DateTime date);
        IHotelBooking GetBooking(int roomNumber, DateTime date);
        IEnumerable<int> GetAvailableRooms(DateTime date);
        IHotelBookingResult UpdateBooking(IHotelBooking hotelBooking);
        IHotelBookingResult DeleteBooking(IHotelBooking hotelBooking);
    }
}