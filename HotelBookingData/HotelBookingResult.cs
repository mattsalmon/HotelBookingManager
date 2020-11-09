using System;

namespace HotelBookingData
{
    public class HotelBookingResult: IHotelBookingResult
    {
        public bool OperationSuccessful {get;set;}
        public string ReasonCode {get;set;}
    }
}