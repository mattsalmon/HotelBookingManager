using System;

namespace HotelBookingTests
{
    public static class DateTimeExtensions
    {
        public static DateTime NextDayOfWeek(this DateTime date, DayOfWeek dayOfWeek)
        {
            DateTime nextDate = date;
            while (nextDate.DayOfWeek != dayOfWeek)
                nextDate = nextDate.AddDays(1D);

            return nextDate;
        }
    }
}