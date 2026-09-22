using System.Text.Json;

namespace AppointmentBooking.Metrics;

public sealed class QualityTargets
{
    public double MinimumValidSuccessRatePercent { get; init; }

    public double MaximumP95LatencyMs { get; init; }

    public double MinimumPeakHourSuccessRatePercent { get; init; }

    public static QualityTargets Load(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException("The quality-target file was not found.", path);
        }

        string json = File.ReadAllText(path);
        var targets = JsonSerializer.Deserialize<QualityTargets>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidDataException("The quality-target file could not be read.");

        if (targets.MinimumValidSuccessRatePercent is < 0 or > 100
            || targets.MinimumPeakHourSuccessRatePercent is < 0 or > 100
            || targets.MaximumP95LatencyMs <= 0)
        {
            throw new InvalidDataException("One or more quality targets are outside their valid range.");
        }

        return targets;
    }
}

