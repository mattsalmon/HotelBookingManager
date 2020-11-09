using System;

namespace HotelBookingData
{
    public class HotelBooking: IHotelBooking
    {
        public string Surname {get;set;}
        public int RoomNumber {get;set;}
        public DateTime BookingDate {get;set;}
    }
}