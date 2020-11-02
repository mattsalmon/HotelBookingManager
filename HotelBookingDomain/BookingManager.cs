﻿using System;
using System.Collections.Generic;
using HotelBookingData;

namespace HotelBookingDomain
{
    public class BookingManager : IBookingManager
    {
        private IHotelBookingRepository _bookingData;
        public BookingManager(IHotelBookingRepository bookingData)
        {
            _bookingData = bookingData;
        }
        public bool IsRoomAvailable(int roomNumber, DateTime date)
        {
            // Check if the given room number is available on that specific date
            var booking = _bookingData.GetBooking(roomNumber, date);
            // If there's no record returned, then the room's available
            if(booking is null)
                return true;
            // Otherwise false
            return false;
        }
        
        public void AddBooking(string surname, int roomNumber, DateTime date)
        {
            var bookingResult = _bookingData.CreateBooking(surname, roomNumber, date);
            if(!bookingResult.OperationSuccessful)
                throw new RoomNotAvailableException($"Room number '{roomNumber}' could not be booked on '{date.ToShortDateString()}'. Reason code given was '{bookingResult.ReasonCode}'.");
        }

        public IEnumerable<int> getAvailableRooms(DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}