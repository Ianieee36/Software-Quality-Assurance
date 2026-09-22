using AppointmentBooking.Metrics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppointmentBooking.Metrics.Tests;

[TestClass]
public class MetricsCalculatorTests
{
    [TestMethod]
    public void ValidSuccessRate_ExcludesInvalidRequests()
    {
        BookingEvent[] events =
        [
            Event("R1", true, "Success", "Normal", 400),
            Event("R2", true, "SystemFailure", "Normal", 900),
            Event("R3", false, "RejectedInvalid", "Normal", 40),
        ];

        double result = MetricsCalculator.ValidSuccessRate(events);
        InconclusiveUntilImplemented(double.IsNaN(result), "ValidSuccessRate");

        Assert.AreEqual(50.0, result, 0.001);
    }

    [TestMethod]
    public void Percentile_UsesNearestRankDefinition()
    {
        double result = MetricsCalculator.Percentile([100, 200, 300, 400, 500], 0.80);
        InconclusiveUntilImplemented(double.IsNaN(result), "Percentile");

        Assert.AreEqual(400.0, result, 0.001);
    }

    [TestMethod]
    public void ValidSuccessRateByTimeBand_SeparatesNormalAndPeakRequests()
    {
        BookingEvent[] events =
        [
            Event("R1", true, "Success", "Normal", 400),
            Event("R2", true, "Success", "Normal", 500),
            Event("R3", true, "Success", "Peak", 700),
            Event("R4", true, "SystemFailure", "Peak", 1200),
            Event("R5", false, "RejectedInvalid", "Peak", 50),
        ];

        IReadOnlyDictionary<string, double> result =
            MetricsCalculator.ValidSuccessRateByTimeBand(events);
        InconclusiveUntilImplemented(result.Count == 0, "ValidSuccessRateByTimeBand");

        Assert.AreEqual(100.0, result["Normal"], 0.001);
        Assert.AreEqual(50.0, result["Peak"], 0.001);
    }

    private static BookingEvent Event(
        string requestId,
        bool isValidRequest,
        string outcome,
        string timeBand,
        double latencyMs)
    {
        return new BookingEvent(
            DateTimeOffset.Parse("2026-08-01T00:00:00Z"),
            "2.3.0",
            requestId,
            isValidRequest,
            outcome,
            timeBand,
            "Web",
            latencyMs);
    }

    private static void InconclusiveUntilImplemented(bool isPlaceholder, string methodName)
    {
        if (isPlaceholder)
        {
            Assert.Inconclusive(
                $"{methodName} is intentionally incomplete in the starter. Complete Activity 3 and run the test again.");
        }
    }
}
