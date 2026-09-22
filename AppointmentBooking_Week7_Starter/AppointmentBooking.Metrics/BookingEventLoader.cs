using System.Globalization;

namespace AppointmentBooking.Metrics;

public static class BookingEventLoader
{
    private static readonly string[] RequiredHeaders =
    [
        "timestampUtc",
        "release",
        "requestId",
        "isValidRequest",
        "outcome",
        "timeBand",
        "channel",
        "latencyMs",
    ];

    public static IReadOnlyList<BookingEvent> Load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The booking-event data file was not found.", path);
        }

        using var reader = File.OpenText(path);
        string? headerLine = reader.ReadLine();
        if (headerLine is null)
        {
            throw new InvalidDataException("The booking-event data file is empty.");
        }

        string[] headers = SplitControlledCsvLine(headerLine);
        var indexes = headers
            .Select((header, index) => (header, index))
            .ToDictionary(item => item.header, item => item.index, StringComparer.OrdinalIgnoreCase);

        foreach (string requiredHeader in RequiredHeaders)
        {
            if (!indexes.ContainsKey(requiredHeader))
            {
                throw new InvalidDataException($"Required column '{requiredHeader}' is missing.");
            }
        }

        var events = new List<BookingEvent>();
        int lineNumber = 1;
        while (reader.ReadLine() is { } line)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            string[] values = SplitControlledCsvLine(line);
            if (values.Length != headers.Length)
            {
                throw new InvalidDataException(
                    $"Line {lineNumber} contains {values.Length} values; {headers.Length} were expected.");
            }

            try
            {
                events.Add(new BookingEvent(
                    DateTimeOffset.Parse(
                        values[indexes["timestampUtc"]],
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.AssumeUniversal),
                    values[indexes["release"]],
                    values[indexes["requestId"]],
                    bool.Parse(values[indexes["isValidRequest"]]),
                    values[indexes["outcome"]],
                    values[indexes["timeBand"]],
                    values[indexes["channel"]],
                    double.Parse(values[indexes["latencyMs"]], CultureInfo.InvariantCulture)));
            }
            catch (Exception exception) when (
                exception is FormatException or ArgumentException or OverflowException)
            {
                throw new InvalidDataException(
                    $"Line {lineNumber} contains an invalid value: {exception.Message}",
                    exception);
            }
        }

        if (events.Count == 0)
        {
            throw new InvalidDataException("The booking-event data file contains no observations.");
        }

        return events;
    }

    private static string[] SplitControlledCsvLine(string line)
    {
        // The supplied synthetic dataset deliberately contains no quoted commas.
        return line.Split(',', StringSplitOptions.TrimEntries);
    }
}

