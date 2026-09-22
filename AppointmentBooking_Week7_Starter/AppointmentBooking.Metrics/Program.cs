using AppointmentBooking.Metrics;

try
{
    CliOptions options = CliOptions.Parse(args);
    IReadOnlyList<BookingEvent> allEvents = BookingEventLoader.Load(options.EventsPath);
    QualityTargets targets = QualityTargets.Load(options.TargetsPath);

    BookingEvent mostRecentEvent = allEvents.MaxBy(item => item.TimestampUtc)
        ?? throw new InvalidOperationException("No booking events were loaded.");
    string currentRelease = mostRecentEvent.Release;
    BookingEvent[] currentEvents = allEvents
        .Where(item => string.Equals(item.Release, currentRelease, StringComparison.OrdinalIgnoreCase))
        .ToArray();

    Console.WriteLine($"Current release: {currentRelease}");
    Console.WriteLine($"Raw observations: {currentEvents.Length}");
    Console.WriteLine($"Valid requests: {currentEvents.Count(item => item.IsValidRequest)}");
    Console.WriteLine($"Invalid requests: {currentEvents.Count(item => !item.IsValidRequest)}");

    foreach (IGrouping<string, BookingEvent> outcomeGroup in currentEvents
        .GroupBy(item => item.Outcome, StringComparer.OrdinalIgnoreCase)
        .OrderBy(group => group.Key, StringComparer.OrdinalIgnoreCase))
    {
        Console.WriteLine($"Outcome {outcomeGroup.Key}: {outcomeGroup.Count()}");
    }

    QualityReportResult result = QualityReportWriter.Write(
        options.OutputPath,
        currentRelease,
        currentEvents,
        targets);

    Console.WriteLine($"Quality summary written to {options.OutputPath}");
    if (!result.HasCompleteMetrics)
    {
        Console.WriteLine("Activity 3 calculations are not complete yet.");
    }

    return 0;
}
catch (Exception exception)
{
    Console.Error.WriteLine($"Metrics reporter error: {exception.Message}");
    return 2;
}
