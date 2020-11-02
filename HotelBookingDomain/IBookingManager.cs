using System;
using System.Collections.Generic;

namespace HotelBookingDomain
{
    public interface IBookingManager
    {
        /**
        * Return true if there is no booking for the given room on the date,
        * otherwise false
        */
        bool IsRoomAvailable(int room, DateTime date);

        /**
        * Add a booking for the given guest in the given room on the given
        * date. If the room is not available, throw a suitable Exception.
        */
        void AddBooking(string guest, int room, DateTime date);

        /**
        * Return a list of all the available room numbers for the given date
        */
        IEnumerable<int> getAvailableRooms(DateTime date);
    }
}