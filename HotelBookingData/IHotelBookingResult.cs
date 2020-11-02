using System;

namespace HotelBookingData
{
    public interface IHotelBookingResult
    {
        bool OperationSuccessful {get;set;}
        string ReasonCode {get;set;}
    }
}