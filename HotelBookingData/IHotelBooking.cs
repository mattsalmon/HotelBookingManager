using System;

namespace HotelBookingData
{
    public interface IHotelBooking
    {
        string Surname {get;set;}
        int RoomNumber {get;set;}
        DateTime BookingDate {get;set;}
    }
}