using System.Globalization;
using System.Text;

namespace AppointmentBooking.Metrics;

public sealed record QualityReportResult(bool HasCompleteMetrics, bool AllTargetsMet);

public static class QualityReportWriter
{
    public static QualityReportResult Write(
        string outputPath,
        string release,
        IReadOnlyList<BookingEvent> events,
        QualityTargets targets)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(release);
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(targets);

        if (events.Count == 0)
        {
            throw new InvalidOperationException("A report cannot be produced without observations.");
        }

        BookingEvent[] validEvents = events.Where(item => item.IsValidRequest).ToArray();
        double validSuccessRate = MetricsCalculator.ValidSuccessRate(events);
        double p50Latency = MetricsCalculator.Percentile(validEvents.Select(item => item.LatencyMs), 0.50);
        double p95Latency = MetricsCalculator.Percentile(validEvents.Select(item => item.LatencyMs), 0.95);
        IReadOnlyDictionary<string, double> byTimeBand =
            MetricsCalculator.ValidSuccessRateByTimeBand(events);

        bool hasNormal = byTimeBand.TryGetValue("Normal", out double normalSuccessRate);
        bool hasPeak = byTimeBand.TryGetValue("Peak", out double peakSuccessRate);
        bool complete = IsCalculated(validSuccessRate)
            && IsCalculated(p50Latency)
            && IsCalculated(p95Latency)
            && hasNormal
            && hasPeak;

        bool allTargetsMet = complete
            && validSuccessRate >= targets.MinimumValidSuccessRatePercent
            && p95Latency <= targets.MaximumP95LatencyMs
            && peakSuccessRate >= targets.MinimumPeakHourSuccessRatePercent;

        var report = new StringBuilder();
        report.AppendLine("# Appointment Booking Quality Summary");
        report.AppendLine();
        report.AppendLine($"- Release: `{release}`");
        report.AppendLine($"- Total observations: {events.Count}");
        report.AppendLine($"- Valid requests: {validEvents.Length}");
        report.AppendLine($"- Invalid requests: {events.Count - validEvents.Length}");
        report.AppendLine();
        report.AppendLine("| Indicator | Current value | Target | Result |");
        report.AppendLine("|---|---:|---:|---|");
        report.AppendLine(CreateHigherIsBetterRow(
            "Valid-request success rate",
            validSuccessRate,
            targets.MinimumValidSuccessRatePercent,
            "%"));
        report.AppendLine(CreateLowerIsBetterRow(
            "P95 latency",
            p95Latency,
            targets.MaximumP95LatencyMs,
            " ms"));
        report.AppendLine(CreateHigherIsBetterRow(
            "Peak-hour valid success",
            hasPeak ? peakSuccessRate : double.NaN,
            targets.MinimumPeakHourSuccessRatePercent,
            "%"));
        report.AppendLine();
        report.AppendLine("## Contextual indicators");
        report.AppendLine();
        report.AppendLine($"- P50 latency: {FormatValue(p50Latency, " ms")}");
        report.AppendLine($"- Normal-hour valid success: {FormatValue(hasNormal ? normalSuccessRate : double.NaN, "%")}");
        report.AppendLine($"- Peak-hour valid success: {FormatValue(hasPeak ? peakSuccessRate : double.NaN, "%")}");
        report.AppendLine();
        report.AppendLine(complete
            ? $"**Target comparison:** {(allTargetsMet ? "all configured targets are met" : "one or more configured targets are not met")}."
            : "**Target comparison:** not evaluated because Activity 3 calculations are incomplete.");

        string? directory = Path.GetDirectoryName(outputPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(outputPath, report.ToString());
        return new QualityReportResult(complete, allTargetsMet);
    }

    private static string CreateHigherIsBetterRow(
        string name,
        double value,
        double target,
        string suffix)
    {
        string result = IsCalculated(value) ? (value >= target ? "PASS" : "FAIL") : "NOT CALCULATED";
        return $"| {name} | {FormatValue(value, suffix)} | >= {FormatValue(target, suffix)} | {result} |";
    }

    private static string CreateLowerIsBetterRow(
        string name,
        double value,
        double target,
        string suffix)
    {
        string result = IsCalculated(value) ? (value <= target ? "PASS" : "FAIL") : "NOT CALCULATED";
        return $"| {name} | {FormatValue(value, suffix)} | <= {FormatValue(target, suffix)} | {result} |";
    }

    private static bool IsCalculated(double value)
    {
        return !double.IsNaN(value) && !double.IsInfinity(value);
    }

    private static string FormatValue(double value, string suffix)
    {
        return IsCalculated(value)
            ? value.ToString("0.##", CultureInfo.InvariantCulture) + suffix
            : "not calculated";
    }
}
