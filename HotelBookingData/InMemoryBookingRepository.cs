using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace HotelBookingData
{
    public class InMemoryBookingRepository : IHotelBookingRepository
    {
        private readonly SqliteConnection _connection;
        public InMemoryBookingRepository()
        {
            const string connectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared";
            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            var createCommand = _connection.CreateCommand();
            createCommand.CommandText =
            @"
                CREATE TABLE booking (
                    Surname TEXT,
                    RoomNumber INT,
                    BookingDate DATETIME
                )
            ";
            createCommand.ExecuteNonQuery();

        }
        public IHotelBookingResult CreateBooking(string surname, int roomNumber, DateTime date)
        {
            IHotelBookingResult result = new HotelBookingResult();
            var command = _connection.CreateCommand();
            command.CommandText =
                @"
                    INSERT INTO booking
                    VALUES (@surname, @roomNumber, @bookingDate)
                ";
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@roomNumber", roomNumber);
            command.Parameters.AddWithValue("@bookingDate", date);
            var rowsInserted = command.ExecuteNonQuery();

            if(rowsInserted == 1)
                result = new HotelBookingResult(){
                    OperationSuccessful = true,
                    ReasonCode = "INSERTED"
                };
            else
                result = new HotelBookingResult(){
                    OperationSuccessful = false,
                    ReasonCode = "MULTIPLE"
                };
            return result;
        }
        

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        public IEnumerable<int> GetAvailableRooms(DateTime date)
        {
            throw new NotImplementedException();
        }

        public IHotelBooking GetBooking(int roomNumber, DateTime date)
        {
            IHotelBooking booking = null;
            var queryCommand = _connection.CreateCommand();
            int totalRecordsReturned = 0;
            queryCommand.CommandText =
            @"
                SELECT Surname, RoomNumber, BookingDate
                FROM booking
                WHERE RoomNumber = @roomNumber
                AND
                BookingDate = @bookingDate
            ";
            queryCommand.Parameters.AddWithValue("@roomNumber", roomNumber);
            queryCommand.Parameters.AddWithValue("@bookingDate", date);
            var reader = queryCommand.ExecuteReader();
            if (reader.HasRows) {
                while(reader.Read())
                        {   
                            totalRecordsReturned++;
                            booking = new HotelBooking(){
                                Surname = Convert.ToString(reader["Surname"]),
                                RoomNumber = Convert.ToInt32(reader["RoomNumber"]),
                                BookingDate = Convert.ToDateTime(reader["BookingDate"])
                            };
                        }
            }

            reader.Close();
            return booking;
        }
    }
}