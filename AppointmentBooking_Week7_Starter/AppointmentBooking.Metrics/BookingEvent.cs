namespace AppointmentBooking.Metrics;

public sealed record BookingEvent(
    DateTimeOffset TimestampUtc,
    string Release,
    string RequestId,
    bool IsValidRequest,
    string Outcome,
    string TimeBand,
    string Channel,
    double LatencyMs);

