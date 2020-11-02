using System;

public class RoomNotAvailableException : Exception
{
    public RoomNotAvailableException()
    {
    }

    public RoomNotAvailableException(string message)
        : base(message)
    {
    }

    public RoomNotAvailableException(string message, Exception inner)
        : base(message, inner)
    {
    }
}