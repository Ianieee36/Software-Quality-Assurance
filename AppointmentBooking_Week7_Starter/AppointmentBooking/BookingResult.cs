namespace ENSE707_AppointmentBooking;

public class BookingResult
{
    public BookingResult(bool success, string message)
    {
        Success = success;
        Message = message;
    }

    public bool Success { get; }

    public string Message { get; }
}

